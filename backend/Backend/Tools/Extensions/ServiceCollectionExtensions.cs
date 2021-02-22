using System.Runtime.CompilerServices;
using System.Text.Json;
using Core.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using SendGrid;
using Serilog;
using Services;
using Services.Google;

namespace Backend.Tools.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddCrossviewServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddPreRoutingCrossviewServices(config)
					.AddRoutingCrossviewServices()
					.AddPostRoutingCrossviewServices()
					.AddCrossviewOptions(config)
					.AddCrossviewApplicationServices(config);
		}

		public static IServiceCollection AddPreRoutingCrossviewServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddSingleton(Log.Logger)
					.AddCrossviewIdentity(config);
			return services;
		}

		public static IServiceCollection AddRoutingCrossviewServices(this IServiceCollection services)
		{
			services.AddRouting()
					.AddControllers();

			return services;
		}

		public static IServiceCollection AddPostRoutingCrossviewServices(this IServiceCollection services)
		{
			// Changes the invalid model state response
			// (this must come after services.AddControllers() in order to have any effect)
			return services.Configure<ApiBehaviorOptions>(options =>
			{
				
				options.InvalidModelStateResponseFactory = context => new UnprocessableEntityObjectResult(new ApiResponse(context.ModelState));
			});
			return services;
		}

		public static IServiceCollection AddCrossviewOptions(this IServiceCollection services, IConfiguration config)
		{
			return services.Configure<EmailOptions>(config.GetSection("Application:Email"));
		}

		public static IServiceCollection AddCrossviewApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddTransient<ISendGridClient>(a => new SendGridClient(config["Application:Email:SendGridApiKey"]))
					.AddScoped<IUserRepository, UserRepository>()
					.AddScoped<IEmailSenderService, EmailSenderService>()
					.AddHttpClient<RecaptchaService>();

			return services;
		}
	}
}