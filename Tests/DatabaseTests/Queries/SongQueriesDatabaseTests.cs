using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VocaDb.Model.Database.Queries;
using VocaDb.Model.Database.Repositories;
using VocaDb.Model.DataContracts.ReleaseEvents;
using VocaDb.Model.DataContracts.UseCases;
using VocaDb.Model.DataContracts.Users;
using VocaDb.Model.Domain.Globalization;
using VocaDb.Model.Domain.ReleaseEvents;
using VocaDb.Model.Service.Helpers;
using VocaDb.Model.Utils.Config;
using VocaDb.Tests.TestData;
using VocaDb.Tests.TestSupport;
using VocaDb.Web.Code;
using VocaDb.Web.Helpers;

namespace VocaDb.Tests.DatabaseTests.Queries
{
	/// <summary>
	/// Database tests for <see cref="SongQueries"/>.
	/// </summary>
	[TestClass]
	public class SongQueriesDatabaseTests
	{
		private readonly DatabaseTestContext<ISongRepository> context = new DatabaseTestContext<ISongRepository>();
		private readonly FakePermissionContext userContext;
		private TestDatabase Db => TestContainerManager.TestDatabase;

		public SongQueriesDatabaseTests()
		{
			userContext = new FakePermissionContext(new UserWithPermissionsContract(Db.UserWithEditPermissions, ContentLanguagePreference.Default));
		}

		private SongQueries Queries(ISongRepository repository)
		{
			return new SongQueries(repository, userContext, new FakeEntryLinkFactory(), new FakePVParser(),
				new FakeUserMessageMailer(), new FakeLanguageDetector(), new FakeUserIconFactory(), new EnumTranslations(), new InMemoryImagePersister(), new FakeObjectCache(), new VdbConfigManager(), new EntrySubTypeNameFactory(),
				new FollowedArtistNotifier(new FakeEntryLinkFactory(), new FakeUserMessageMailer(), new EnumTranslations(), new EntrySubTypeNameFactory()));
		}

		private async Task<SongForEditContract> Update(SongForEditContract contract)
		{
			return await context.RunTestAsync(async repository =>
			{
				var queries = Queries(repository);

				var updated = await queries.UpdateBasicProperties(contract);

				return queries.GetSongForEdit(updated.Id);
			});
		}

		[TestMethod]
		[TestCategory(TestCategories.Database)]
		public async Task Update_ReleaseEvent_Remove()
		{
			// Preconditions (arrange)
			Assert.IsNotNull(Db.Song.ReleaseEvent, "ReleaseEvent");
			Assert.IsTrue(Db.ReleaseEvent.AllSongs.Contains(Db.Song), "Release event has song");

			// Act
			var contract = new SongForEditContract(Db.Song, ContentLanguagePreference.English)
			{
				ReleaseEvent = null
			};

			await context.RunTestAsync(async repository =>
			{
				var queries = Queries(repository);

				var updated = await queries.UpdateBasicProperties(contract);

				// Assert
				Assert.IsNull(updated.ReleaseEvent, "Release event was cleared");
				var releaseEvent = repository.HandleQuery(ctx => ctx.Load<ReleaseEvent>(Db.ReleaseEvent.Id));
				Assert.AreEqual(0, releaseEvent.AllSongs.Count, "Song was removed from event");
			});
		}

		[TestMethod]
		[TestCategory(TestCategories.Database)]
		public async Task Update_ReleaseEvent_Change()
		{
			await context.RunTestAsync(async repository =>
			{
				var queries = Queries(repository);

				var newEvent = repository.HandleTransaction(ctx => new ReleaseEventContract(ctx.Save(CreateEntry.ReleaseEvent("Mikumas")), ContentLanguagePreference.English, false));

				// Act
				var contract = new SongForEditContract(Db.Song, ContentLanguagePreference.English)
				{
					ReleaseEvent = newEvent
				};

				var updated = await queries.UpdateBasicProperties(contract);

				// Assert
				Assert.AreEqual(newEvent.Id, updated.ReleaseEvent?.Id, "Release event was changed");
				var releaseEvent = repository.HandleQuery(ctx => ctx.Load<ReleaseEvent>(newEvent.Id));
				Assert.AreEqual(1, releaseEvent.AllSongs.Count, "Song was added to event");
			});
		}

		[TestMethod]
		[TestCategory(TestCategories.Database)]
		public async Task Update_Lyrics()
		{
			var contract = new SongForEditContract(Db.Song2, ContentLanguagePreference.English)
			{
				Lyrics = new[] { CreateEntry.LyricsForSongContract(TranslationType.Original) }
			};

			var song = await Update(contract);

			Assert.AreEqual(1, song.Lyrics.Length, "Lyrics created");
		}
	}
}
