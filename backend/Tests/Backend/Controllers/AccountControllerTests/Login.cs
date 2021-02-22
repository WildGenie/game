using System;
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
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Tests.Backend.Controllers.AccountControllerTests
{
	public class Login
	{
		[Fact]
		public async Task ReturnsNotFoundIfUserNotExists()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(userExists: false);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var login = new LoginModel
			{
				AccountName = "crossview",
				Password = "something secure"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.Login(login);

			Assert.NotNull(result);
			Assert.IsType<NotFoundObjectResult>(result);
			Assert.IsType<ApiResponse>((result as NotFoundObjectResult)?.Value);

			var response = (result as NotFoundObjectResult).Value as ApiResponse ?? new ApiResponse(new List<IdentityError> {new IdentityErrorDescriber().DefaultError()});

			Assert.Contains(nameof(login.AccountName), response.Errors.Keys);
			Assert.Single(response.Errors[nameof(login.AccountName)]);
			Assert.Equal("An account with that name was not found.", response.Errors[nameof(login.AccountName)][0]);
		}

		[Fact]
		public async Task ReturnsForbiddenIfUserLockedOut()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(userLockedOut: true);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var login = new LoginModel
			{
				AccountName = "crossview",
				Password = "something secure"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.Login(login);

			Assert.NotNull(result);
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ApiResponse>((result as ObjectResult)?.Value);

			var response = (result as ObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains("", response.Errors.Keys);
			Assert.Single(response.Errors[""]);
			Assert.StartsWith("The specified user account is currently locked out. The lockout will end", response.Errors[""][0]);
		}

		[Fact]
		public async Task ReturnsUnauthorizedObjectResultIfRecaptchaFails()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(accessFailedCount: 3);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.FailingRecaptchaService();

			var login = new LoginModel
			{
				AccountName = "crossview",
				Password = "something secure",
				RecaptchaToken = "Google sent me"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.Login(login);

			Assert.NotNull(result);
			Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnauthorizedObjectResult)?.Value);

			var response = (result as UnauthorizedObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains(nameof(login.RecaptchaToken), response.Errors.Keys);
			Assert.Single(response.Errors[nameof(login.RecaptchaToken)]);
			Assert.Equal(ErrorMessages.RecaptchaNoConnection, response.Errors[nameof(login.RecaptchaToken)][0]);
		}

		[Fact]
		public async Task ReturnsUnauthorizedIfEmailNotConfirmed()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(emailConfirmed: false);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var login = new LoginModel
			{
				AccountName = "crossview",
				Password = "something secure"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.Login(login);

			Assert.NotNull(result);
			Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnauthorizedObjectResult)?.Value);

			var response = (result as UnauthorizedObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains("", response.Errors.Keys);
			Assert.Single(response.Errors[""]);
			Assert.Equal(ErrorMessages.LoginFailedNotConfirmed, response.Errors[""][0]);
		}

		[Fact]
		public async Task ReturnsApplicationUserModelIfSucceeded()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var login = new LoginModel
			{
				AccountName = "crossview",
				Password = "something secure"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.Login(login);

			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
			Assert.IsType<ApiResponse<ApplicationUserModel>>((result as OkObjectResult)?.Value);

			var response = (result as OkObjectResult).Value as ApiResponse<ApplicationUserModel> ?? new ApiResponse<ApplicationUserModel>();
			var user = response.Result;
			
			Assert.Equal(login.AccountName, user.UserName);
		}

		[Fact]
		public async Task ReturnsUnauthorizedIfPasswordInvalid()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService(passwordCorrect: false);
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.SucceedingRecaptchaService();

			var login = new LoginModel
			{
				AccountName = "crossview",
				Password = "something secure"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.Login(login);

			Assert.NotNull(result);
			Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnauthorizedObjectResult)?.Value);

			var response = (result as UnauthorizedObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains("", response.Errors.Keys);
			Assert.Single(response.Errors[""]);
			Assert.Equal(ErrorMessages.LoginFailedInvalid, response.Errors[""][0]);
		}

		[Fact]
		public async Task ReturnsForbiddenIfLockedOut()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService(passwordCorrect: false, signInResult: SignInResult.LockedOut);
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.SucceedingRecaptchaService();

			var login = new LoginModel
			{
				AccountName = "crossview",
				Password = "something secure",
				RecaptchaToken = "Google sent me"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.Login(login);

			Assert.NotNull(result);
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ApiResponse>((result as ObjectResult)?.Value);

			var response = (result as ObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains("", response.Errors.Keys);
			Assert.Single(response.Errors[""]);
			Assert.Equal(ErrorMessages.LoginFailedLocked, response.Errors[""][0]);
		}

		[Fact]
		public async Task ReturnsNeverIfLockoutEndMax()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(userLockedOut: true, lockoutEnd: DateTimeOffset.MaxValue);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var login = new LoginModel
			{
				AccountName = "crossview",
				Password = "something secure"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.Login(login);

			Assert.NotNull(result);
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ApiResponse>((result as ObjectResult)?.Value);

			var response = (result as ObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains("", response.Errors.Keys);
			Assert.Single(response.Errors[""]);
			Assert.Equal("The specified user account is currently locked out. The lockout will end never", response.Errors[""][0]);
		}
		
		[Fact]
		public async Task ReturnsDateStringIfLockedOutTemporarily()
		{
			var lockoutOutEnd = DateTime.Now.AddDays(1);
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(userLockedOut: true, lockoutEnd: lockoutOutEnd);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.RecaptchaService();

			var login = new LoginModel
			{
				AccountName = "crossview",
				Password = "something secure"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.Login(login);

			Assert.NotNull(result);
			Assert.IsType<ObjectResult>(result);
			Assert.IsType<ApiResponse>((result as ObjectResult)?.Value);

			var response = (result as ObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains("", response.Errors.Keys);
			Assert.Single(response.Errors[""]);
			Assert.Equal($"The specified user account is currently locked out. The lockout will end {lockoutOutEnd:D} at {lockoutOutEnd:h:mm:ss tt} UTC", response.Errors[""][0]);
		}

		[Fact]
		public async Task RequiresRecaptchaIfAccessFailedGreaterEqualTwo()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(accessFailedCount: 3);
			var signInService = ServiceMockFactory.SignInService(passwordCorrect: false);
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.SucceedingRecaptchaService();

			var login = new LoginModel
			{
				AccountName = "crossview",
				Password = "something secure",
				RecaptchaToken = "Google sent me"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.Login(login);

			Assert.NotNull(result);
			Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnauthorizedObjectResult)?.Value);

			var response = (result as UnauthorizedObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains(nameof(login.RecaptchaToken), response.Errors.Keys);
			Assert.Single(response.Errors[nameof(login.RecaptchaToken)]);
			Assert.Equal("Please complete the reCAPTCHA challenge as an additional security measure", response.Errors[nameof(login.RecaptchaToken)][0]);
		}

		[Fact]
		public async Task ReturnsUnauthorizedIfEmailNotConfirmedButPasswordNotCorrect()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(emailConfirmed: false, passwordCorrect: false);
			var signInService = ServiceMockFactory.SignInService(passwordCorrect: false);
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.SucceedingRecaptchaService();

			var login = new LoginModel
			{
				AccountName = "crossview",
				Password = "something secure"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.Login(login);

			Assert.NotNull(result);
			Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnauthorizedObjectResult)?.Value);

			var response = (result as UnauthorizedObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains("", response.Errors.Keys);
			Assert.Single(response.Errors[""]);
			Assert.Equal(ErrorMessages.LoginFailedInvalid, response.Errors[""][0]);
		}
		
		[Fact]
		public async Task ReturnsUnauthorizedIfRequires2Fa()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService(passwordCorrect: false, signInResult: SignInResult.TwoFactorRequired);
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.SucceedingRecaptchaService();

			var login = new LoginModel
			{
				AccountName = "crossview",
				Password = "something secure",
				RecaptchaToken = "Google sent me"
			};

			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.Login(login);

			Assert.NotNull(result);
			Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnauthorizedObjectResult)?.Value);

			var response = (result as UnauthorizedObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			Assert.Contains("", response.Errors.Keys);
			Assert.Single(response.Errors[""]);
			Assert.Equal(ErrorMessages.LoginFailed2fa, response.Errors[""][0]);
		}
	}
}