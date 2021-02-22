using System.Threading.Tasks;
using Core.DataModels;
using Repositories;
using Tests.Mocks;
using Xunit;

namespace Tests.Repositories.RepositoryTests
{
	public class GetAll
	{
		[Fact]
		public async Task ReturnsAllUsers()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb();
			var repo = new Repository<ApplicationUser, string>(context);
			var users = await repo.GetAll();

			Assert.Equal(5, users.Count);
			Assert.Equal("crossview1", users[0].UserName);
		}

		[Fact]
		public async Task ReturnsEmptyListIfNoUsers()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb(false);
			var repo = new Repository<ApplicationUser, string>(context);
			
			Assert.Empty(await repo.GetAll());
		}
	}
}