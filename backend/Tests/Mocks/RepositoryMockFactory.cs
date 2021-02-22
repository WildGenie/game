using System.Collections.Generic;
using Core.DataModels;
using Moq;
using Repositories;

namespace Tests.Mocks
{
	public static class RepositoryMockFactory
	{
		public static Mock<IUserRepository> UserRepository(bool userExists = true, int userCount = 1)
		{
			var repo = new Mock<IUserRepository>();

			var findById = repo.Setup(r => r.FindById(It.IsAny<string>()));
			var getPagified = repo.Setup(r => r.GetPagified(1, 10));

			if (userExists)
			{
				var user = new ApplicationUser
				{
					UserName = "bastion",
					ConcurrencyStamp = "Don't commit if I'm not the same!",
					Email = "no-reply@bastionofshadows.com",
					EmailConfirmed = true,
					Id = "something unique",
					NormalizedUserName = "BASTION",
					NormalizedEmail = "NO-REPLY@bastionofshadows.com",
					AccessFailedCount = 0,
					LockoutEnd = null,
					PendingEmail = string.Empty
				};

				findById.ReturnsAsync(user);
				getPagified.ReturnsAsync(new List<ApplicationUser>{ user });
			}

			repo.Setup(r => r.GetCount())
				.ReturnsAsync(userCount);

			return repo;
		}
	}
}