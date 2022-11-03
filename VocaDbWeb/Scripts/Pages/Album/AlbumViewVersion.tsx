import Breadcrumb from '@/Bootstrap/Breadcrumb';
import SafeAnchor from '@/Bootstrap/SafeAnchor';
import { Layout } from '@/Components/Shared/Layout';
import { PrintArchivedAlbumData } from '@/Components/Shared/Partials/Album/PrintArchivedAlbumData';
import { ArchivedObjectVersionProperties } from '@/Components/Shared/Partials/ArchivedEntry/ArchivedObjectVersionProperties';
import { HiddenBanner } from '@/Components/Shared/Partials/EntryDetails/HiddenBanner';
import { ReportEntryVersionPopupKnockout } from '@/Components/Shared/Partials/EntryDetails/ReportEntryVersionPopupKnockout';
import { useChangedFieldNames } from '@/Components/useChangedFieldNames';
import { useVdbTitle } from '@/Components/useVdbTitle';
import { ArchivedAlbumVersionDetailsContract } from '@/DataContracts/Album/ArchivedAlbumVersionDetailsContract';
import JQueryUIButton from '@/JQueryUI/JQueryUIButton';
import { EntryType } from '@/Models/EntryType';
import { LoginManager } from '@/Models/LoginManager';
import { AlbumRepository } from '@/Repositories/AlbumRepository';
import { EntryUrlMapper } from '@/Shared/EntryUrlMapper';
import { HttpClient } from '@/Shared/HttpClient';
import { ArchivedAlbumStore } from '@/Stores/Album/ArchivedAlbumStore';
import { useLocationStateStore } from '@vocadb/route-sphere';
import { runInAction } from 'mobx';
import { observer } from 'mobx-react-lite';
import qs from 'qs';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { Link, useParams, useSearchParams } from 'react-router-dom';

const loginManager = new LoginManager(vdb.values);

const httpClient = new HttpClient();

const albumRepo = new AlbumRepository(httpClient, vdb.values.baseAddress);

interface AlbumViewVersionLayoutProps {
	contract: ArchivedAlbumVersionDetailsContract;
	archivedAlbumStore: ArchivedAlbumStore;
}

const AlbumViewVersionLayout = observer(
	({
		contract,
		archivedAlbumStore,
	}: AlbumViewVersionLayoutProps): React.ReactElement => {
		const { t } = useTranslation(['ViewRes']);

		const title = `Revision ${contract.archivedVersion.version} for ${contract.name}`; /* LOCALIZE */

		useVdbTitle(title, true);

		const changedFieldNames = useChangedFieldNames();

		useLocationStateStore(archivedAlbumStore);

		return (
			<Layout
				title={title}
				parents={
					<>
						<Breadcrumb.Item
							linkAs={Link}
							linkProps={{
								to: '/Album',
							}}
							divider
						>
							{t(`ViewRes:Shared.Albums`)}
						</Breadcrumb.Item>
						<Breadcrumb.Item
							linkAs={Link}
							linkProps={{
								to: EntryUrlMapper.details(EntryType.Album, contract.album.id),
							}}
							divider
						>
							{contract.album.name}
						</Breadcrumb.Item>
						<Breadcrumb.Item
							linkAs={Link}
							linkProps={{
								to: `/Album/Versions/${contract.album.id}`,
							}}
						>
							Revisions{/* LOCALIZE */}
						</Breadcrumb.Item>
					</>
				}
				toolbar={
					<>
						{loginManager.canRestoreRevisions &&
							contract.archivedVersion.version < contract.album.version - 1 && (
								<JQueryUIButton
									as="a"
									href={
										`/Album/RevertToVersion?${qs.stringify({
											archivedAlbumVersionId: contract.archivedVersion.id,
										})}` /* TODO: Convert to POST. */
									}
									onClick={(e): void => {
										if (
											!window.confirm(
												t('ViewRes:ViewVersion.ConfirmRevertToVersion'),
											)
										) {
											e.preventDefault();
										}
									}}
									icons={{ primary: 'ui-icon-arrowrefresh-1-w' }}
								>
									{t('ViewRes:ViewVersion.RevertToVersion')}
								</JQueryUIButton>
							)}{' '}
						&nbsp;{' '}
						<JQueryUIButton
							as="a"
							href={`/Album/ArchivedVersionXml/${contract.archivedVersion.id}`}
							icons={{ primary: 'ui-icon-arrowthickstop-1-s' }}
						>
							Download XML{/* LOCALIZE */}
						</JQueryUIButton>{' '}
						<JQueryUIButton
							as={SafeAnchor}
							href="#"
							onClick={archivedAlbumStore.reportStore.show}
							icons={{ primary: 'ui-icon-alert' }}
						>
							{t('ViewRes:EntryDetails.ReportAnError')}
						</JQueryUIButton>{' '}
						{loginManager.canViewHiddenRevisions &&
							(contract.archivedVersion.hidden ? (
								<JQueryUIButton
									as="a"
									href={`/Album/UpdateVersionVisibility?${qs.stringify({
										archivedVersionId: contract.archivedVersion.id,
										hidden: false,
									})}`} /* TODO: Convert to POST. */
									onClick={(e): void => {
										if (
											!window.confirm(t('ViewRes:ViewVersion.ConfirmUnhide'))
										) {
											e.preventDefault();
										}
									}}
									icons={{ primary: 'ui-icon-unlocked' }}
								>
									{t('ViewRes:ViewVersion.UnhideVersion')}
								</JQueryUIButton>
							) : (
								<JQueryUIButton
									as="a"
									href={`/Album/UpdateVersionVisibility?${qs.stringify({
										archivedVersionId: contract.archivedVersion.id,
										hidden: true,
									})}`} /* TODO: Convert to POST. */
									onClick={(e): void => {
										if (!window.confirm(t('ViewRes:ViewVersion.ConfirmHide'))) {
											e.preventDefault();
										}
									}}
									icons={{ primary: 'ui-icon-locked' }}
								>
									{t('ViewRes:ViewVersion.HideVersion')}
								</JQueryUIButton>
							))}
					</>
				}
			>
				{contract.archivedVersion.hidden && <HiddenBanner />}

				{contract.comparableVersions.length > 0 && (
					<form className="form form-inline">
						Compare to:{/* LOCALIZE */}{' '}
						<select
							className="input-xlarge"
							value={archivedAlbumStore.comparedVersionId}
							onChange={(e): void =>
								runInAction(() => {
									archivedAlbumStore.comparedVersionId = e.target.value
										? Number(e.target.value)
										: undefined;
								})
							}
						>
							<option value="" />
							{contract.comparableVersions.map((comparableVersion) => (
								<option
									value={comparableVersion.id}
									key={comparableVersion.id}
								>{`${
									comparableVersion.version
								} (${comparableVersion.changedFields
									.map((changedField) =>
										changedFieldNames(EntryType.Album, changedField),
									)
									.join(', ')} by ${comparableVersion.agentName})`}</option>
							))}
						</select>
					</form>
				)}

				<ArchivedObjectVersionProperties
					version={contract.archivedVersion}
					compareTo={contract.comparedVersion}
					entryType={EntryType.Album}
				/>

				<PrintArchivedAlbumData comparedAlbums={contract.versions} />

				<ReportEntryVersionPopupKnockout
					reportEntryStore={archivedAlbumStore.reportStore}
				/>
			</Layout>
		);
	},
);

const AlbumViewVersion = (): React.ReactElement => {
	const { id } = useParams();
	const [searchParams] = useSearchParams();
	const comparedVersionId = searchParams.get('comparedVersionId');

	const [model, setModel] = React.useState<{
		contract: ArchivedAlbumVersionDetailsContract;
		archivedAlbumStore: ArchivedAlbumStore;
	}>();

	React.useEffect(() => {
		albumRepo
			.getVersionDetails({
				id: Number(id),
				comparedVersionId: comparedVersionId
					? Number(comparedVersionId)
					: undefined,
			})
			.then((contract) =>
				setModel({
					contract: contract,
					archivedAlbumStore: new ArchivedAlbumStore(
						contract.album.id,
						contract.archivedVersion.version,
						albumRepo,
					),
				}),
			);
	}, [id, comparedVersionId]);

	return model ? (
		<AlbumViewVersionLayout
			contract={model.contract}
			archivedAlbumStore={model.archivedAlbumStore}
		/>
	) : (
		<></>
	);
};

export default AlbumViewVersion;
