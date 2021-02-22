using System.Net;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core;
using Core.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Services;
using Tests.Mocks;
using Xunit;
using Xunit.Sdk;

namespace Tests.Backend.Controllers.AccountControllerTests
{
	public class PerformEmailChange
	{
		[Fact]
		public async Task LogsPerformingEmailChangeMessage()
		{
			var logger = LoggerMockFactory.InformationLoggerWithThreeArgs("Performing email change for user {userId} to {newEmail} with token {verificationCode}");
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendEmailChangeConfirmationEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var emailChange = new PerformEmailChangeModel
			{
				NewEmail = "no-reply@crossviewsoftware.io",
				UserId = "abc123",
				VerificationCode = "xyz789"
			};
			
			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			await controller.PerformEmailChange(emailChange);

			var logged = false;
			try
			{
				logger.Verify();
				logged = true;
			}
			catch (MockException) { }

			Assert.True(logged);
		}

		[Fact]
		public async Task ReturnsConflictObjectResultIfEmailsNotMatch()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(pendingEmail: "old@crossviewsoftware.io");
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendEmailChangeConfirmationEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var emailChange = new PerformEmailChangeModel
			{
				NewEmail = "no-reply2@crossviewsoftware.io",
				UserId = "abc123",
				VerificationCode = "xyz789"
			};
			
			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.PerformEmailChange(emailChange);
			
			Assert.NotNull(result);
			Assert.IsType<ConflictObjectResult>(result);
			Assert.IsType<ApiResponse>((result as ConflictObjectResult)?.Value);

			var response = (result as ConflictObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains("", response.Errors.Keys);
			Assert.Single(response.Errors[""]);
			Assert.Equal(ErrorMessages.EmailErrorWrongEmail, response.Errors[""][0]);
		}

		[Fact]
		public async Task ReturnsForbiddenIfUserIdsNotMatch()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(pendingEmail: "old@crossviewsoftware.io");
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendEmailChangeConfirmationEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var emailChange = new PerformEmailChangeModel
			{
				NewEmail = "old@crossviewsoftware.io",
				UserId = "abc123",
				VerificationCode = "xyz789"
			};
			
			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.PerformEmailChange(emailChange);
			
			Assert.NotNull(result);
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ApiResponse>((result as ObjectResult)?.Value);
			Assert.Equal(403, (result as ObjectResult)?.StatusCode);

			var response = (result as ObjectResult)?.Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains("", response.Errors.Keys);
			Assert.Single(response.Errors[""]);
			Assert.Equal(ErrorMessages.AccountErrorWrongId, response.Errors[""][0]);
		}

		[Fact]
		public async Task ReturnsNoContentIfSuccessful()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(pendingEmail: "old@crossviewsoftware.io");
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendEmailChangeConfirmationEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var emailChange = new PerformEmailChangeModel
			{
				NewEmail = "old@crossviewsoftware.io",
				UserId = "something unique",
				VerificationCode = "xyz789"
			};
			
			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.PerformEmailChange(emailChange);
			
			Assert.NotNull(result);
			Assert.IsType<NoContentResult>(result);
		}

		[Fact]
		public async Task ReturnsUnprocessableEntityObjectResultIfNotSucceeded()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(pendingEmail: "old@crossviewsoftware.io", verificationCodeMatches: false);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendEmailChangeConfirmationEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var emailChange = new PerformEmailChangeModel
			{
				NewEmail = "old@crossviewsoftware.io",
				UserId = "something unique",
				VerificationCode = "xyz789"
			};
			
			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.PerformEmailChange(emailChange);
			
			Assert.NotNull(result);
			Assert.IsType<UnprocessableEntityObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnprocessableEntityObjectResult)?.Value);

			var response = (result as UnprocessableEntityObjectResult)?.Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains("General", response.Errors.Keys);
			Assert.Single(response.Errors["General"]);
			Assert.Equal("Invalid token.", response.Errors["General"][0]);
		}
	}
}