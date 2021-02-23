using System.Net.Http;
using Backend;
using Backend.Tools.Extensions;
using Core.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Repositories;
using Repositories.Characters;
using SendGrid;
using Serilog;
using Services;
using Services.Characters;
using Services.Google;
using Xunit;

namespace Tests.Backend.Tools.Extensions.ServiceCollectionExtensionsTests
{
	public class AddBastionServices
	{
		[Fact]
		public void CallsAddPreRoutingBastionServices()
		{
			var config = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json")
						 .Build();
			var services = new ServiceCollection();
			var target = new Startup(config);
			target.ConfigureServices(services);
			services.AddBastionServices(config);
			var container = services.BuildServiceProvider();

			var logger = container.GetService<ILogger>();
			Assert.NotNull(logger);
		}
		
		[Fact]
		public void CallsAddRoutingBastionServices()
		{
			var config = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json")
						 .Build();
			var services = new ServiceCollection();
			var target = new Startup(config);
			target.ConfigureServices(services);
			services.AddBastionServices(config);
			var container = services.BuildServiceProvider();

			// Test AddRouting()
			var parser = container.GetService<LinkParser>();
			Assert.NotNull(parser);
			
			// Test AddControllers()
			var cors = container.GetService<CorsAuthorizationFilter>();
			Assert.NotNull(cors);
		}
		
		[Fact]
		public void CallsAddPostRoutingBastionServices()
		{
			var config = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json")
						 .Build();
			var services = new ServiceCollection();
			var target = new Startup(config);
			target.ConfigureServices(services);
			services.AddBastionServices(config);
			var container = services.BuildServiceProvider();

			var options = container.GetService<IOptions<ApiBehaviorOptions>>();
			Assert.NotNull(options);

			var context = new ActionContext();
			var result = options.Value.InvalidModelStateResponseFactory(context);

			Assert.NotNull(result);
			Assert.IsType<UnprocessableEntityObjectResult>(result);
		}
		
		[Fact]
		public void CallsAddBastionOptions()
		{
			var config = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json")
						 .Build();
			var services = new ServiceCollection();
			var target = new Startup(config);
			target.ConfigureServices(services);
			services.AddBastionServices(config);
			var container = services.BuildServiceProvider();

			var options = container.GetService<IOptions<EmailOptions>>();
			Assert.NotNull(options);
		}
		
		[Fact]
		public void CallsAddBastionApplicationServices()
		{
			var config = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json")
						 .Build();
			var services = new ServiceCollection();
			var target = new Startup(config);
			target.ConfigureServices(services);
			services.AddBastionServices(config);
			services.AddSingleton<IConfiguration>(config);
			var container = services.BuildServiceProvider();

			var sendGridClient = container.GetService<ISendGridClient>();
			Assert.NotNull(sendGridClient);

			var userRepo = container.GetService<IUserRepository>();
			Assert.NotNull(userRepo);

			var speciesRepo = container.GetService<ISpeciesRepository>();
			Assert.NotNull(speciesRepo);

			var speciesService = container.GetService<ISpeciesService>();
			Assert.NotNull(speciesService);

			var emailSenderService = container.GetService<IEmailSenderService>();
			Assert.NotNull(emailSenderService);

			var recaptchaService = container.GetService<RecaptchaService>();
			Assert.NotNull(recaptchaService);
		}
	}
}