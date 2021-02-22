using System.Threading.Tasks;
using Backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.AccountControllerTests
{
	public class Logout
	{
		[Fact]
		public async Task ReturnsNoContent()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.Logout();

			Assert.NotNull(result);
			Assert.IsType<NoContentResult>(result);
		}

		[Fact]
		public async Task CallsSignOutAsync()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			await controller.Logout();

			var wasCalled = false;
			try
			{
				signInService.Verify();
				wasCalled = true;
			}
			catch (MockException) { }

			Assert.True(wasCalled);
		}
	}
}