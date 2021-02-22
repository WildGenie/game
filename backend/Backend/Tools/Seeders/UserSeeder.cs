using System.Threading.Tasks;
using Core.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Serilog;
using Services.Identity;

namespace Backend.Tools.Seeders
{
	public class UserSeeder : DatabaseSeeder<UserService>
	{
		private readonly IConfiguration _config;

		public UserSeeder(UserService service, IConfiguration config, ILogger logger) : base(service, logger)
		{
			_config = config;
		}

		public override async Task SeedData()
		{
			Logger.Information("No application users found. Seeding new user");

			var user = new ApplicationUser
			{
				Email = _config["Application:Defaults:UserEmail"],
				UserName = _config["Application:Defaults:UserName"]
			};

			var result = await Service.CreateAsync(user, _config["Application:Defaults:UserPassword"]);
			if (result.Succeeded)
			{
				var token = await Service.GenerateEmailConfirmationTokenAsync(user);
				await Service.ConfirmEmailAsync(user, token);
			}
			else
			{
				Logger.Error("Failed to seed application user: {@errors}", result.Errors);
			}
		}
	}
}