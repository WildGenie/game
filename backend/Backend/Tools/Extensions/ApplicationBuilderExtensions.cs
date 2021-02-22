using System.Diagnostics.CodeAnalysis;
using Core.DataModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Serilog;
using Services.Identity;

namespace Backend.Tools.Extensions
{
	// Excluded from coverage
	// The middleware pipeline can't be unit tested effectively
	[ExcludeFromCodeCoverage]
	public static class ApplicationBuilderExtensions
	{
		public static void ConfigureBastionApplication(this IApplicationBuilder app)
		{
			app.UseSerilogRequestLogging();

			app.EnsureDatabaseSeeded();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}

		public static void EnsureDatabaseSeeded(this IApplicationBuilder app)
		{
			using var serviceScope = app.ApplicationServices
										.GetRequiredService<IServiceScopeFactory>()
										.CreateScope();

			var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
			var userService = serviceScope.ServiceProvider.GetService<UserService>();
			var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();
			var logger = serviceScope.ServiceProvider.GetService<ILogger>();
			var config = serviceScope.ServiceProvider.GetService<IConfiguration>();

			context.SeedDatabase(userService, roleManager, config, logger);
		}
	}
}