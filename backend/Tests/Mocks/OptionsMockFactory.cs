using Core.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Tests.Mocks
{
	public class OptionsMockFactory
	{
		public static IOptions<EmailOptions> EmailOptions()
		{
			var emailOptions = new EmailOptions
			{
				EmailFromAddress = "no-reply@bastionofshadows.com",
				EmailFromName = "Bastion of Shadows",
				SendGridApiKey = "5up3453c43t",
				ApplicationUrl = "http://localhost:8080"
			};
			return Options.Create(emailOptions);
		}

		public static IOptions<ApiBehaviorOptions> ApiBehaviorOptions()
		{
			var options = new ApiBehaviorOptions {InvalidModelStateResponseFactory = context => new UnprocessableEntityObjectResult(context.ModelState)};

			return Options.Create(options);
		}

		public static IOptions<IdentityOptions> IdentityOptions() => Options.Create(new IdentityOptions());
	}
}