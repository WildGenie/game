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
				Email = "no-reply5@bastionofshadows.com",
				EmailConfirmed = true,
				Id = "abc1235",
				UserName = "bastion5",
				NormalizedEmail = "NO-REPLY5@bastionofshadows.com",
				NormalizedUserName = "BASTION5"
			});

			var user = await repo.FindById("abc1235");
			
			Assert.NotNull(user);
			Assert.Equal("bastion5", user.UserName);
		}
	}
}