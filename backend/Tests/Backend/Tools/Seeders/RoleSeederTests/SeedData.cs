using System.Threading.Tasks;
using Backend.Tools.Seeders;
using Moq;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Tools.Seeders.RoleSeederTests
{
	public class SeedData
	{
		[Fact]
		public async Task LogsInformationWhenSeedingData()
		{
			var service = ServiceMockFactory.RoleManager();
			var logger = LoggerMockFactory.InformationLogger("No application roles found. Seeding role data");

			var seeder = new RoleSeeder(service.Object, logger.Object);
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
			var service = ServiceMockFactory.RoleManager(roleCreated: false);
			var logger = LoggerMockFactory.ErrorLoggerWithSingleArg("Failed to seed application role: {@errors}");

			var seeder = new RoleSeeder(service.Object, logger.Object);
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