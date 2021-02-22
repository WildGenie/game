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
				Email = "no-reply1@crossviewsoftware.io",
				EmailConfirmed = true,
				Id = "abc1231",
				UserName = "crossview1",
				NormalizedEmail = "NO-REPLY1@CROSSVIEWSOFTWARE.IO",
				NormalizedUserName = "CROSSVIEW1"
			});
			
			context.Users.Add(new ApplicationUser
			{
				Email = "no-reply2@crossviewsoftware.io",
				EmailConfirmed = true,
				Id = "abc1232",
				UserName = "crossview2",
				NormalizedEmail = "NO-REPLY2@CROSSVIEWSOFTWARE.IO",
				NormalizedUserName = "CROSSVIEW2"
			});
			
			context.Users.Add(new ApplicationUser
			{
				Email = "no-reply3@crossviewsoftware.io",
				EmailConfirmed = true,
				Id = "abc1233",
				UserName = "crossview3",
				NormalizedEmail = "NO-REPLY3@CROSSVIEWSOFTWARE.IO",
				NormalizedUserName = "CROSSVIEW3"
			});
			
			context.Users.Add(new ApplicationUser
			{
				Email = "no-reply4@crossviewsoftware.io",
				EmailConfirmed = true,
				Id = "abc1234",
				UserName = "crossview4",
				NormalizedEmail = "NO-REPLY4@CROSSVIEWSOFTWARE.IO",
				NormalizedUserName = "CROSSVIEW4"
			});
			
			context.Users.Add(new ApplicationUser
			{
				Email = "no-reply5@crossviewsoftware.io",
				EmailConfirmed = true,
				Id = "abc1235",
				UserName = "crossview5",
				NormalizedEmail = "NO-REPLY5@CROSSVIEWSOFTWARE.IO",
				NormalizedUserName = "CROSSVIEW5"
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