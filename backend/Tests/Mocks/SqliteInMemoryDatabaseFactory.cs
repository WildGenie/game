using Core.DataModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Tests.Mocks
{
	internal static class SqliteInMemoryDatabaseFactory
	{
		public static ApplicationDbContext GetNewDb(bool seedDb = true)
		{
			var connection = new SqliteConnection("Filename=:memory:");
			connection.Open();

			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
						  .UseSqlite(connection)
						  .Options;

			var context = new ApplicationDbContext(options);
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			if (seedDb)
				Seed(context);
			
			return context;
		}

		private static void Seed(ApplicationDbContext context)
		{
			SeedUsers(context);
			SeedRoles(context);
			SeedUserRoles(context);
		}

		private static void SeedUsers(ApplicationDbContext context)
		{
			context.Users.Add(new ApplicationUser
			{
				Email = "no-reply1@bastionofshadows.com",
				EmailConfirmed = true,
				Id = "abc1231",
				UserName = "bastion1",
				NormalizedEmail = "NO-REPLY1@bastionofshadows.com",
				NormalizedUserName = "BASTION1"
			});
			
			context.Users.Add(new ApplicationUser
			{
				Email = "no-reply2@bastionofshadows.com",
				EmailConfirmed = true,
				Id = "abc1232",
				UserName = "bastion2",
				NormalizedEmail = "NO-REPLY2@bastionofshadows.com",
				NormalizedUserName = "BASTION2"
			});
			
			context.Users.Add(new ApplicationUser
			{
				Email = "no-reply3@bastionofshadows.com",
				EmailConfirmed = true,
				Id = "abc1233",
				UserName = "bastion3",
				NormalizedEmail = "NO-REPLY3@bastionofshadows.com",
				NormalizedUserName = "BASTION3"
			});
			
			context.Users.Add(new ApplicationUser
			{
				Email = "no-reply4@bastionofshadows.com",
				EmailConfirmed = true,
				Id = "abc1234",
				UserName = "bastion4",
				NormalizedEmail = "NO-REPLY4@bastionofshadows.com",
				NormalizedUserName = "BASTION4"
			});
			
			context.Users.Add(new ApplicationUser
			{
				Email = "no-reply5@bastionofshadows.com",
				EmailConfirmed = true,
				Id = "abc1235",
				UserName = "bastion5",
				NormalizedEmail = "NO-REPLY5@bastionofshadows.com",
				NormalizedUserName = "BASTION5"
			});
			
			context.SaveChanges();
		}

		private static void SeedRoles(ApplicationDbContext context)
		{
			context.Roles.Add(new Role("Administrator")
			{
				Id = "xyz789",
				NormalizedName = "ADMINISTRATOR"
			});
			context.SaveChanges();
		}

		private static void SeedUserRoles(ApplicationDbContext context)
		{
			context.UserRoles.Add(new UserRole
			{
				RoleId = "xyz789",
				UserId = "abc1231"
			});
			context.SaveChanges();
		}
	}
}