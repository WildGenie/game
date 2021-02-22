using System.Threading.Tasks;
using Core.DataModels;
using Repositories;
using Tests.Mocks;
using Xunit;

namespace Tests.Repositories.RepositoryTests
{
	public class FindByProperty
	{
		[Fact]
		public async Task ReturnsEntityIfExists()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb();
			var repo = new Repository<ApplicationUser, string>(context);
			var user = await repo.FindByProperty(u => u.Email == "no-reply3@crossviewsoftware.io");
			
			Assert.NotNull(user);
			Assert.Equal("crossview3", user.UserName);
		}

		[Fact]
		public async Task ReturnsNullIfEntityNotExists()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb(false);
			var repo = new Repository<ApplicationUser, string>(context);
			var user = await repo.FindByProperty(u => u.Email == "no-reply@crossviewsoftware.io");
			
			Assert.Null(user);
		}
	}
}