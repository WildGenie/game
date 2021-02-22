using System.Threading.Tasks;
using Core.DataModels;
using Repositories;
using Tests.Mocks;
using Xunit;

namespace Tests.Repositories.RepositoryTests
{
	public class Update
	{
		[Fact]
		public async Task UpdatesEntity()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb();
			var repo = new Repository<ApplicationUser, string>(context);
			var user = await repo.FindById("abc1231");
			user.Email = "no-reply6@bastionofshadows.com";
			await repo.Update(user);

			var updatedUser = await repo.FindById("abc1231");
			
			Assert.Equal("no-reply6@bastionofshadows.com", updatedUser.Email);
		}
	}
}