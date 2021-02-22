using System.Threading.Tasks;
using Backend.Tools.Seeders;
using Moq;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Tools.Seeders.UserRoleSeederTests
{
	public class SeedData
	{
		[Fact]
		public async Task LogsInfoWhenSeeding()
		{
			var userService = ServiceMockFactory.UserService();
			var config = ConfigurationMockFactory.DatabaseSeedConfiguration();
			var logger = LoggerMockFactory.InformationLogger("Adding scaffolded user to roles");

			var seeder = new UserRoleSeeder(userService.Object, config.Object, logger.Object);
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
		public async Task LogsErrorWhenSeedingFails()
		{
			var service = ServiceMockFactory.UserService(addToRoleSuccessful: false);
			var config = ConfigurationMockFactory.DatabaseSeedConfiguration();
			var logger = LoggerMockFactory.ErrorLoggerWithSingleArg("Failed to seed user with role: {@errors}");

			var seeder = new UserRoleSeeder(service.Object, config.Object, logger.Object);
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