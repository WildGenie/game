using System.Threading.Tasks;
using Core.DataModels;
using Repositories;
using Tests.Mocks;
using Xunit;

namespace Tests.Repositories.RepositoryTests
{
	public class FindById
	{
		[Fact]
		public async Task ReturnsEntityWhenPresent()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb();
			var repo = new Repository<ApplicationUser, string>(context);
			var user = await repo.FindById("abc1231");
			
			Assert.NotNull(user);
			Assert.Equal("crossview1", user.UserName);
		}

		[Fact]
		public async Task ReturnsNullWhenEntityNotPresent()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb(false);
			var repo = new Repository<ApplicationUser, string>(context);
			var user = await repo.FindById("abc123");
			
			Assert.Null(user);
		}
	}
}