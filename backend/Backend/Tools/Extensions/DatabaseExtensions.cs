using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.Tools.Seeders;
using Core.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repositories;
using Serilog;
using Services.Identity;

namespace Backend.Tools.Extensions
{
	public static class DatabaseExtensions
	{
		public static void SeedDatabase(this ApplicationDbContext context, UserService userService, RoleManager<Role> roleManager, IConfiguration config, ILogger logger)
		{
			try
			{
				Task.Run(async () =>
					{
						var shouldSeedUsers = !context.Set<ApplicationUser>()
													  .Any();
						if (shouldSeedUsers)
						{
							var userSeeder = new UserSeeder(userService, config, logger);
							await userSeeder.SeedData();
						}

						if (!context.Set<Role>()
									.Any())
						{
							var roleSeeder = new RoleSeeder(roleManager, logger);
							await roleSeeder.SeedData();
						}

						if (shouldSeedUsers)
						{
							var userRoleSeeder = new UserRoleSeeder(userService, config, logger);
							await userRoleSeeder.SeedData();
						}
					})
					.Wait();
			}
			catch (Exception e)
			{
				logger.Error("Seeding database failed: {@error}", e);
			}
		}
	}
}