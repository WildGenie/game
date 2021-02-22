using System.Threading.Tasks;
using Core.DataModels;
using Repositories;
using Tests.Mocks;
using Xunit;

namespace Tests.Repositories.RepositoryTests
{
	public class GetCount
	{
		[Fact]
		public async Task ReturnsTheCorrectCountWhenEmpty()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb(false);
			var repo = new Repository<ApplicationUser, string>(context);
			
			Assert.Equal(0, await repo.GetCount());
		}

		[Fact]
		public async Task ReturnsTheCorrectCountWhenNotEmpty()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb();
			var repo = new Repository<ApplicationUser, string>(context);
			
			Assert.Equal(5, await repo.GetCount());
		}
	}
}