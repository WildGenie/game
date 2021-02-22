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
	public class ResetPassword
	{
		[Fact]
		public async Task ReturnsNotFoundIfUserNotExists()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(userExists: false);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendPasswordResetEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var resetPasswordModel = new ResetPasswordModel
			{
				NewPassword = "SomethingStrong!1",
				ConfirmNewPassword = "SomethingStrong!1",
				UserId = "Something unique",
				VerificationCode = "abc123"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.ResetPassword(resetPasswordModel);

			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task ReturnsNoContentIfSucceeded()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendPasswordResetEmailSubject);
			var recaptchaService = ServiceMockFactory.SucceedingRecaptchaService();

			var resetPasswordModel = new ResetPasswordModel
			{
				NewPassword = "SomethingStrong!1",
				ConfirmNewPassword = "SomethingStrong!1",
				UserId = "Something unique",
				VerificationCode = "abc123"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.ResetPassword(resetPasswordModel);

			Assert.NotNull(result);
			Assert.IsType<NoContentResult>(result);
		}

		[Fact]
		public async Task ReturnsUnprocessableEntityResultIfNotSuccessful()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(newPasswordValid: false);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendPasswordResetEmailSubject);
			var recaptchaService = ServiceMockFactory.SucceedingRecaptchaService();

			var resetPasswordModel = new ResetPasswordModel
			{
				NewPassword = "SomethingStrong!1",
				ConfirmNewPassword = "SomethingStrong!1",
				UserId = "Something unique",
				VerificationCode = "abc123"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.ResetPassword(resetPasswordModel);

			Assert.NotNull(result);
			Assert.IsType<UnprocessableEntityObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnprocessableEntityObjectResult)?.Value);

			var response = (result as UnprocessableEntityObjectResult).Value as ApiResponse ?? new ApiResponse(new List<IdentityError>());

			Assert.Contains("Password", response.Errors.Keys);
			Assert.Single(response.Errors["Password"]);
			Assert.Equal("Passwords must have at least one digit ('0'-'9').", response.Errors["Password"][0]);
		}
	}
}