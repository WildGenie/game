using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core;
using Core.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.AccountControllerTests
{
	public class DeleteAccount
	{
		[Fact]
		public async Task ReturnsUnauthorizedIfPasswordIncorrect()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(passwordCorrect: false);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendPasswordResetEmailSubject);
			var recaptchaService = ServiceMockFactory.SucceedingRecaptchaService();

			var deleteAccount = new DeleteAccountModel
			{
				ConfirmPassword = "abc123"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.DeleteAccount(deleteAccount);
			
			Assert.NotNull(result);
			Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnauthorizedObjectResult)?.Value);

			var response = (result as UnauthorizedObjectResult)?.Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains("", response.Errors.Keys);
			Assert.Single(response.Errors[""]);
			Assert.Equal(ErrorMessages.LoginFailedInvalid, response.Errors[""][0]);
		}

		[Fact]
		public async Task ReturnsNoContentIfSuccessful()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendPasswordResetEmailSubject);
			var recaptchaService = ServiceMockFactory.SucceedingRecaptchaService();

			var deleteAccount = new DeleteAccountModel
			{
				ConfirmPassword = "abc123"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.DeleteAccount(deleteAccount);
			
			Assert.NotNull(result);
			Assert.IsType<NoContentResult>(result);
		}

		[Fact]
		public async Task ReturnsUnprocessableEntityResultIfFailed()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(deleteSuccessful: false);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendPasswordResetEmailSubject);
			var recaptchaService = ServiceMockFactory.SucceedingRecaptchaService();

			var deleteAccount = new DeleteAccountModel
			{
				ConfirmPassword = "abc123"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.DeleteAccount(deleteAccount);
			
			Assert.NotNull(result);
			Assert.IsType<UnprocessableEntityObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnprocessableEntityObjectResult)?.Value);

			var response = (result as UnprocessableEntityObjectResult)?.Value as ApiResponse ?? new ApiResponse(new List<IdentityError>());

			Assert.Contains("General", response.Errors.Keys);
			Assert.Single(response.Errors["General"]);
			Assert.Equal("Optimistic concurrency failure, object has been modified.", response.Errors["General"][0]);
		}
	}
}