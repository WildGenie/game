using System.Diagnostics.CodeAnalysis;
using Backend.Tools.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			// All services have been added through extension methods
			// found in Backend.Tools.Extensions.ServiceCollectionExtensions
			services.AddBastionServices(Configuration);
		}

		// Excluded from coverage
		// The middleware pipeline can't be unit tested effectively
		[ExcludeFromCodeCoverage]
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// All services have been configured through extension methods
			// found in Backend.Tools.Extensions.ApplicationBuilderExtensions
			app.ConfigureBastionApplication();
		}
	}
}