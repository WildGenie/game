using System.Threading.Tasks;
using Core.DataModels;
using Repositories;
using Tests.Mocks;
using Xunit;

namespace Tests.Repositories.RepositoryTests
{
	public class FindManyByProperty
	{
		[Fact]
		public async Task ReturnsMatches()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb();
			var repo = new Repository<ApplicationUser, string>(context);
			var users = await repo.FindManyByProperty(u => u.Email.StartsWith("no-reply"));
			
			Assert.NotEmpty(users);
			Assert.Equal(5, users.Count);
		}
	}
}