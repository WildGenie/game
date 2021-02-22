using System.Threading.Tasks;
using Core.DataModels;
using Repositories;
using Tests.Mocks;
using Xunit;

namespace Tests.Repositories.RepositoryTests
{
	public class GetPagified
	{
		[Fact]
		public async Task ReturnsPagifiedResults()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb();
			var repo = new Repository<ApplicationUser, string>(context);
			var result1 = await repo.GetPagified(1, 2);
			
			Assert.Equal(2, result1.Count);
			Assert.Equal("crossview2", result1[1].UserName);

			var result2 = await repo.GetPagified(2, 2);
			
			Assert.Equal(2, result2.Count);
			Assert.Equal("crossview4", result2[1].UserName);
		}

		[Fact]
		public async Task ReturnsPagifiedResultsBasedOnPredicate()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb();
			var repo = new Repository<ApplicationUser, string>(context);
			var result = await repo.GetPagified(1, 2, u => u.UserName.EndsWith("3"));
			
			Assert.Single(result);
			Assert.Equal("crossview3", result[0].UserName);
		}
	}
}