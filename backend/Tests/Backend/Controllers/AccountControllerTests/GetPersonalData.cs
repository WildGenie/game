using System.Threading.Tasks;
using Backend.Controllers;
using Core.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.AccountControllerTests
{
	public class GetPersonalData
	{
		[Fact]
		public async Task ReturnsFileContentResult()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(newPasswordValid: false);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendPasswordResetEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);

			controller.ControllerContext = new ControllerContext();
			controller.ControllerContext.HttpContext = new DefaultHttpContext();

			var result = await controller.GetPersonalData();

			Assert.NotNull(result);
			Assert.IsType<FileContentResult>(result);
		}
	}
}