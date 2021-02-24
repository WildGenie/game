using Core;
using Core.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Repositories.Characters;
using SendGrid;
using Serilog;
using Services;
using Services.Characters;
using Services.Google;

namespace Backend.Tools.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddBastionServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddPreRoutingBastionServices(config)
					.AddRoutingBastionServices()
					.AddPostRoutingBastionServices()
					.AddBastionOptions(config)
					.AddBastionApplicationServices(config);
		}

		public static IServiceCollection AddPreRoutingBastionServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddSingleton(Log.Logger)
					.AddBastionIdentity(config);
			return services;
		}

		public static IServiceCollection AddRoutingBastionServices(this IServiceCollection services)
		{
			services.AddRouting()
					.AddControllers();

			return services;
		}

		public static IServiceCollection AddPostRoutingBastionServices(this IServiceCollection services)
		{
			// Changes the invalid model state response
			// (this must come after services.AddControllers() in order to have any effect)
			return services.Configure<ApiBehaviorOptions>(options => { options.InvalidModelStateResponseFactory = context => new UnprocessableEntityObjectResult(new ApiResponse(context.ModelState)); });
		}

		public static IServiceCollection AddBastionOptions(this IServiceCollection services, IConfiguration config)
		{
			return services.Configure<EmailOptions>(config.GetSection("Application:Email"));
		}

		public static IServiceCollection AddBastionApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddTransient<ISendGridClient>(a => new SendGridClient(config["Application:Email:SendGridApiKey"]))
					.AddTransient<EmailMessages>()
					.AddScoped<IUserRepository, UserRepository>()
					.AddScoped<ISpeciesRepository, SpeciesRepository>()
					.AddScoped<IEmailSenderService, EmailSenderService>()
					.AddScoped<ISpeciesService, SpeciesService>()
					.AddHttpClient<RecaptchaService>();

			return services;
		}
	}
}