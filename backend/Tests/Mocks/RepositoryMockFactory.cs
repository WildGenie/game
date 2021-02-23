using System;
using System.Collections.Generic;
using Core.DataModels;
using Core.DataModels.Characters;
using Moq;
using Repositories;
using Repositories.Characters;

namespace Tests.Mocks
{
	public static class RepositoryMockFactory
	{
		private static readonly Exception MockError = new Exception("Kindly mock this throw");
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
				getPagified.ReturnsAsync(new List<ApplicationUser> {user});
			}

			repo.Setup(r => r.GetCount())
				.ReturnsAsync(userCount);

			return repo;
		}

		public static Mock<ISpeciesRepository> SpeciesRepository(bool successful = true, bool throws = false, bool updateThrows = false)
		{
			var repo = new Mock<ISpeciesRepository>();

			var findById = repo.Setup(s => s.FindById(It.IsAny<int>()));
			var create = repo.Setup(s => s.Create(It.IsAny<Species>()));
			var update = repo.Setup(s => s.Update(It.IsAny<Species>()));

			if (successful)
			{
				var species = new Species
				{
					Id = 1,
					Name = "Human",
					PluralName = "Humans",
					Description = "Humans are cool and good",
					ForceSensitive = true,
					HpCoefficient = 10.0f
				};

				findById.ReturnsAsync(species);
			}
			else
			{
				findById.ReturnsAsync(() => null);
                create.ThrowsAsync(MockError);
			}
			
			if (throws)
			{
				findById.ThrowsAsync(MockError);
			}
			
			if (updateThrows)
			{
				update.ThrowsAsync(MockError);
			}

			repo.Setup(s => s.GetAll())
				.ReturnsAsync(new List<Species>
				{
					new Species
					{
						Id = 1,
						Name = "Human",
						PluralName = "Humans",
						Description = "Humans are cool and good",
						ForceSensitive = true,
						HpCoefficient = 10.0f
					},
					new Species
					{
						Id = 2,
						Name = "Twi'lek",
						PluralName = "Twi'leks",
						Description = "Twi'leks make my wife jealous",
						ForceSensitive = true,
						HpCoefficient = 11.0f
					}
				});

			return repo;
		}
	}
}