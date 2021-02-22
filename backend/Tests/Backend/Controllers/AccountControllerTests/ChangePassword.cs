using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.AccountControllerTests
{
	public class ChangePassword
	{
		[Fact]
		public async Task ReturnsNoContentIfSucceeded()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var changePassword = new ChangePasswordModel
			{
				NewPassword = "SomethingNew1!",
				ConfirmNewPassword = "SomethingNew!",
				CurrentPassword = "My current password"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.ChangePassword(changePassword);

			Assert.NotNull(result);
			Assert.IsType<NoContentResult>(result);
		}

		[Fact]
		public async Task ReturnsUnprocessableEntityObjectResultIfFailed()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(newPasswordValid: false);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var changePassword = new ChangePasswordModel
			{
				NewPassword = "SomethingNew1!",
				ConfirmNewPassword = "SomethingNew!",
				CurrentPassword = "My current password"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.ChangePassword(changePassword);

			Assert.NotNull(result);
			Assert.IsType<UnprocessableEntityObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnprocessableEntityObjectResult)?.Value);

			var response = (result as UnprocessableEntityObjectResult)?.Value as ApiResponse ?? new ApiResponse(new List<IdentityError>());

			Assert.Contains("Password", response.Errors.Keys);
			Assert.Single(response.Errors["Password"]);
			Assert.Equal("Passwords must have at least one digit ('0'-'9').", response.Errors["Password"][0]);
		}
	}
}