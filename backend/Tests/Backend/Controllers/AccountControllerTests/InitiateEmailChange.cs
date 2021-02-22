using System.Threading.Tasks;
using Backend.Controllers;
using Core.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using Services;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.AccountControllerTests
{
	public class InitiateEmailChange
	{
		[Fact]
		public async Task ReturnsUnauthorizedIfPasswordIncorrect()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(passwordCorrect: false);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendEmailChangeConfirmationEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var emailChange = new InitiateEmailChangeModel
			{
				Email = "no-reply@crossviewsoftware.io",
				ConfirmEmail = "no-reply@crossviewsoftware.io",
				ConfirmPassword = "super secure password"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.InitiateEmailChange(emailChange);

			Assert.NotNull(result);
			Assert.IsType<UnauthorizedResult>(result);
		}

		[Fact]
		public async Task ReturnsNoContentIfPasswordCorrect()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendEmailChangeConfirmationEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var emailChange = new InitiateEmailChangeModel
			{
				Email = "no-reply@crossviewsoftware.io",
				ConfirmEmail = "no-reply@crossviewsoftware.io",
				ConfirmPassword = "super secure password"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.InitiateEmailChange(emailChange);

			Assert.NotNull(result);
			Assert.IsType<NoContentResult>(result);
		}
	}
}