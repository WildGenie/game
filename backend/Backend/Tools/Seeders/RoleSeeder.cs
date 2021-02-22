using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.DataModels;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Backend.Tools.Seeders
{
	public class RoleSeeder : DatabaseSeeder<RoleManager<Role>>
	{
		public RoleSeeder(RoleManager<Role> service, ILogger logger) : base(service, logger) { }

		public override async Task SeedData()
		{
			Logger.Information("No application roles found. Seeding role data");

			var roles = new List<string>
			{
				Roles.Admin
			};

			foreach (var roleName in roles)
			{
				var role = new Role(roleName);
				var result = await Service.CreateAsync(role);
				if (!result.Succeeded)
				{
					Logger.Error("Failed to seed application role: {@errors}", result.Errors);
				}
			}
		}
	}
}