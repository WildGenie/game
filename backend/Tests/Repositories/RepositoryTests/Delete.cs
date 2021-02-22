using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataModels;
using Repositories;
using Tests.Mocks;
using Xunit;

namespace Tests.Repositories.RepositoryTests
{
	public class Delete
	{
		[Fact]
		public async Task DeletesEntity()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb();
			var repo = new Repository<ApplicationUser, string>(context);
			await repo.Delete("abc1231");

			var user = await repo.FindById("abc1231");
			
			Assert.Null(user);
		}

		[Fact]
		public async Task ThrowsIfEntityNotExists()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb(false);
			var repo = new Repository<ApplicationUser, string>(context);

			await Assert.ThrowsAsync<KeyNotFoundException>(async () => await repo.Delete("abc1236"));
		}
	}
}