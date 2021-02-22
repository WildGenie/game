using System.Threading.Tasks;
using Core;
using Core.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Serilog;
using Services.Identity;

namespace Backend.Tools.Seeders
{
	public class UserRoleSeeder : DatabaseSeeder<UserService>
	{
		private readonly IConfiguration _config;

		public UserRoleSeeder(UserService service, IConfiguration config, ILogger logger) : base(service, logger)
		{
			_config = config;
		}

		public override async Task SeedData()
		{
			Logger.Information("Adding scaffolded user to roles");
			var user = await Service.FindByNameAsync(_config["Application:Defaults:UserName"]);
			var result = await Service.AddToRoleAsync(user, Roles.Admin);
			if (!result.Succeeded)
			{
				Logger.Error("Failed to seed user with role: {@errors}", result.Errors);
			}
		}
	}
}