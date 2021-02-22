using System.Threading.Tasks;
using Core.DataModels;
using Repositories;
using Tests.Mocks;
using Xunit;

namespace Tests.Repositories.RepositoryTests
{
	public class Create
	{
		[Fact]
		public async Task CreatesEntity()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb(false);
			var repo = new Repository<ApplicationUser, string>(context);
			await repo.Create(new ApplicationUser
			{
				Email = "no-reply5@crossviewsoftware.io",
				EmailConfirmed = true,
				Id = "abc1235",
				UserName = "crossview5",
				NormalizedEmail = "NO-REPLY5@CROSSVIEWSOFTWARE.IO",
				NormalizedUserName = "CROSSVIEW5"
			});

			var user = await repo.FindById("abc1235");
			
			Assert.NotNull(user);
			Assert.Equal("crossview5", user.UserName);
		}
	}
}