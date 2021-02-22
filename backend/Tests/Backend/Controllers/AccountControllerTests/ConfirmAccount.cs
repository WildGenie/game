using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.AccountControllerTests
{
	public class ConfirmAccount
	{
		[Fact]
		public async Task ReturnsNotFoundIfUserNull()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(userExists: false);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var account = new ConfirmAccountModel
			{
				UserId = "abc123",
				VerificationCode = "xyz789"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.ConfirmAccount(account);

			Assert.NotNull(result);
			Assert.IsType<NotFoundObjectResult>(result);
		}

		[Fact]
		public async Task ReturnsNoContentIfSuccessful()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var account = new ConfirmAccountModel
			{
				UserId = "abc123",
				VerificationCode = "xyz789"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.ConfirmAccount(account);

			Assert.NotNull(result);
			Assert.IsType<NoContentResult>(result);
		}
		
		[Fact]
		public async Task ReturnsUnprocessableEntityIfEmailNotConfirmed()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(confirmEmailResult: IdentityResult.Failed(new IdentityErrorDescriber().InvalidToken()));
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var account = new ConfirmAccountModel
			{
				UserId = "abc123",
				VerificationCode = "xyz789"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.ConfirmAccount(account);

			Assert.NotNull(result);
			Assert.IsType<UnprocessableEntityObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnprocessableEntityObjectResult)?.Value);

			var response = (result as UnprocessableEntityObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains("General", response.Errors.Keys);
			Assert.Single(response.Errors["General"]);
			Assert.Equal("Invalid token.", response.Errors["General"][0]);
		}
	}
}