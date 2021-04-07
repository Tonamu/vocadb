#nullable disable

using System.Runtime.Serialization;
using VocaDb.Model.DataContracts.Users;
using VocaDb.Model.Domain.Songs;

namespace VocaDb.Model.DataContracts.Songs
{
	public class ServerOnlyArchivedSongVersionContract : ServerOnlyArchivedObjectVersionContract
	{
		public ServerOnlyArchivedSongVersionContract() { }

		public ServerOnlyArchivedSongVersionContract(ArchivedSongVersion archivedVersion, IUserIconFactory userIconFactory)
			: base(archivedVersion, userIconFactory)
		{
			ChangedFields = (archivedVersion.Diff != null ? archivedVersion.Diff.ChangedFields.Value : SongEditableFields.Nothing);
			Reason = archivedVersion.Reason;
		}

		public SongEditableFields ChangedFields { get; init; }

		public SongArchiveReason Reason { get; init; }
	}
}
