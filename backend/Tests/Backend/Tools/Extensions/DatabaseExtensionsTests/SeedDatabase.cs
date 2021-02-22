using System;
using Backend.Tools.Extensions;
using Core;
using Core.DataModels;
using Moq;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Tools.Extensions.DatabaseExtensionsTests
{
	public class SeedDatabase
	{
		[Fact]
		public void SeedsUsersIfShouldSeed()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb(false);
			var userService = ServiceMockFactory.UserService();
			var roleManager = ServiceMockFactory.RoleManager();
			var config = ConfigurationMockFactory.DatabaseSeedConfiguration();
			var logger = LoggerMockFactory.DefaultLogger();

			context.SeedDatabase(userService.Object, roleManager.Object, config.Object, logger.Object);

			var usersSeeded = false;
			try
			{
				userService.Verify(s => s.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()));
				usersSeeded = true;
			}
			catch (MockException) { }

			Assert.True(usersSeeded);
		}

		[Fact]
		public void DoesNotSeedUsersIfNotShouldSeed()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb();
			var userService = ServiceMockFactory.UserService();
			var roleManager = ServiceMockFactory.RoleManager();
			var config = ConfigurationMockFactory.DatabaseSeedConfiguration();
			var logger = LoggerMockFactory.DefaultLogger();

			context.SeedDatabase(userService.Object, roleManager.Object, config.Object, logger.Object);

			var usersSeeded = true;
			try
			{
				userService.Verify(s => s.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
				usersSeeded = false;
			}
			catch (MockException) { }

			Assert.False(usersSeeded);
		}

		[Fact]
		public void SeedsRolesIfShouldSeed()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb(false);
			var userService = ServiceMockFactory.UserService();
			var roleManager = ServiceMockFactory.RoleManager();
			var config = ConfigurationMockFactory.DatabaseSeedConfiguration();
			var logger = LoggerMockFactory.DefaultLogger();

			context.SeedDatabase(userService.Object, roleManager.Object, config.Object, logger.Object);

			var rolesSeeded = false;
			try
			{
				roleManager.Verify(s => s.CreateAsync(It.IsAny<Role>()));
				rolesSeeded = true;
			}
			catch (MockException) { }

			Assert.True(rolesSeeded);
		}

		[Fact]
		public void DoesNotSeedRolesIfNotShouldSeed()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb();
			var userService = ServiceMockFactory.UserService();
			var roleManager = ServiceMockFactory.RoleManager();
			var config = ConfigurationMockFactory.DatabaseSeedConfiguration();
			var logger = LoggerMockFactory.DefaultLogger();

			context.SeedDatabase(userService.Object, roleManager.Object, config.Object, logger.Object);

			var rolesSeeded = true;
			try
			{
				roleManager.Verify(s => s.CreateAsync(It.IsAny<Role>()), Times.Never);
				rolesSeeded = false;
			}
			catch (MockException) { }

			Assert.False(rolesSeeded);
		}

		[Fact]
		public void SeedsUserRolesIfShouldSeed()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb(false);
			var userService = ServiceMockFactory.UserService();
			var roleManager = ServiceMockFactory.RoleManager();
			var config = ConfigurationMockFactory.DatabaseSeedConfiguration();
			var logger = LoggerMockFactory.DefaultLogger();

			context.SeedDatabase(userService.Object, roleManager.Object, config.Object, logger.Object);

			var userRolesSeeded = false;
			try
			{
				userService.Verify(s => s.AddToRoleAsync(It.IsAny<ApplicationUser>(), Roles.Admin));
				userRolesSeeded = true;
			}
			catch (MockException) { }

			Assert.True(userRolesSeeded);
		}

		[Fact]
		public void DoesNotSeedUserRolesIfNotShouldSeed()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb();
			var userService = ServiceMockFactory.UserService();
			var roleManager = ServiceMockFactory.RoleManager();
			var config = ConfigurationMockFactory.DatabaseSeedConfiguration();
			var logger = LoggerMockFactory.DefaultLogger();

			context.SeedDatabase(userService.Object, roleManager.Object, config.Object, logger.Object);

			var userRolesSeeded = true;
			try
			{
				userService.Verify(s => s.AddToRoleAsync(It.IsAny<ApplicationUser>(), Roles.Admin), Times.Never);
				userRolesSeeded = false;
			}
			catch (MockException) { }

			Assert.False(userRolesSeeded);
		}

		[Fact]
		public void LogsErrorOnException()
		{
			var context = SqliteInMemoryDatabaseFactory.GetNewDb(false);
			var userService = ServiceMockFactory.UserService(createUserThrows: true);
			var roleManager = ServiceMockFactory.RoleManager();
			var config = ConfigurationMockFactory.DatabaseSeedConfiguration();
			var logger = LoggerMockFactory.ErrorLoggerWithException<Exception>("Seeding database failed: {@error}");

			context.SeedDatabase(userService.Object, roleManager.Object, config.Object, logger.Object);

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