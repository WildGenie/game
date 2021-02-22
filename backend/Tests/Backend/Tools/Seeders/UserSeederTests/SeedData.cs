using System.Threading.Tasks;
using Backend.Tools.Seeders;
using Core.DataModels;
using Moq;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Tools.Seeders.UserSeederTests
{
	public class SeedData
	{
		[Fact]
		public async Task LogsInfoWhenSeeding()
		{
			var userService = ServiceMockFactory.UserService();
			var config = ConfigurationMockFactory.DatabaseSeedConfiguration();
			var logger = LoggerMockFactory.InformationLogger("No application users found. Seeding new user");

			var seeder = new UserSeeder(userService.Object, config.Object, logger.Object);
			await seeder.SeedData();

			var logged = false;
			try
			{
				logger.Verify();
				logged = true;
			}
			catch (MockException) { }

			Assert.True(logged);
		}

		[Fact]
		public async Task ConfirmsUserEmailIfResultSucceeded()
		{
			var userService = ServiceMockFactory.UserService();
			var config = ConfigurationMockFactory.DatabaseSeedConfiguration();
			var logger = LoggerMockFactory.DefaultLogger();

			var seeder = new UserSeeder(userService.Object, config.Object, logger.Object);
			await seeder.SeedData();

			var confirmed = false;
			try
			{
				userService.Verify(s => s.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), "your token, sire"));
				confirmed = true;
			}
			catch (MockException) { }

			Assert.True(confirmed);
		}
		
		[Fact]
		public async Task LogsErrorIfSeedingFailed()
		{
			var userService = ServiceMockFactory.UserService(userCreated: false);
			var config = ConfigurationMockFactory.DatabaseSeedConfiguration();
			var logger = LoggerMockFactory.ErrorLoggerWithSingleArg("Failed to seed application user: {@errors}");

			var seeder = new UserSeeder(userService.Object, config.Object, logger.Object);
			await seeder.SeedData();

			var logged = false;
			try
			{
				logger.Verify();
				logged = true;
			}
			catch (MockException) { }

			Assert.True(logged);
		}
	}
}