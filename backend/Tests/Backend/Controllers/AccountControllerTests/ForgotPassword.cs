using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core;
using Core.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.AccountControllerTests
{
	public class ForgotPassword
	{
		[Fact]
		public async Task ReturnsAcceptedIfUserIsNull()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(userExists: false);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendPasswordResetEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var forgotPassword = new ForgotPasswordModel
			{
				AccountName = "bastion",
				RecaptchaToken = "abc123"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.ForgotPassword(forgotPassword);

			Assert.NotNull(result);
			Assert.IsType<AcceptedResult>(result);
		}

		[Fact]
		public async Task ReturnsUnauthorizedIfRecaptchaNotSuccessful()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendPasswordResetEmailSubject);
			var recaptchaService = ServiceMockFactory.FailingRecaptchaService();

			var forgotPassword = new ForgotPasswordModel
			{
				AccountName = "bastion",
				RecaptchaToken = "abc123"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.ForgotPassword(forgotPassword);

			Assert.NotNull(result);
			Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnauthorizedObjectResult)?.Value);

			var response = (result as UnauthorizedObjectResult)?.Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains(nameof(forgotPassword.RecaptchaToken), response.Errors);
			Assert.Single(response.Errors[nameof(forgotPassword.RecaptchaToken)]);
			Assert.Equal(ErrorMessages.RecaptchaNoConnection, response.Errors[nameof(forgotPassword.RecaptchaToken)][0]);
		}

		[Fact]
		public async Task ReturnsAcceptedIfSuccessful()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendPasswordResetEmailSubject);
			var recaptchaService = ServiceMockFactory.SucceedingRecaptchaService();

			var forgotPassword = new ForgotPasswordModel
			{
				AccountName = "bastion",
				RecaptchaToken = "abc123"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.ForgotPassword(forgotPassword);

			Assert.NotNull(result);
			Assert.IsType<AcceptedResult>(result);
		}
	}
}