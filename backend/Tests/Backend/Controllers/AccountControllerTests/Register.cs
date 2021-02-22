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
	public class Register
	{
		[Fact]
		public async Task ReturnsInvalidModelStateResponseIfAcceptTosFalse()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService();
			var recaptchaService = ServiceMockFactory.RecaptchaService();
			
			var register = new RegisterModel
			{
				AcceptTos = false,
				Password = "hello1!...",
				ConfirmPassword = "hello1!...",
				Email = "no-reply@crossviewsoftware.io",
				RecaptchaToken = "abc123!.alkjsadfoijwe",
				UserName = "crossview1"
			};
			
			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.Register(register);

			Assert.NotNull(result);
			Assert.IsType<UnprocessableEntityObjectResult>(result);
			Assert.IsType<SerializableError>((result as UnprocessableEntityObjectResult)?.Value);

			var response = (result as UnprocessableEntityObjectResult).Value as SerializableError ?? new SerializableError();
			var errors = response[nameof(register.AcceptTos)] as string[] ?? new string[0];

			Assert.Contains(nameof(register.AcceptTos), response.Keys);
			Assert.Contains("You must accept the Terms of Service in order to register", errors);
		}

		[Fact]
		public async Task ReturnsInvalidModelStateResponseIfRecaptchaFails()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService();
			var recaptchaService = ServiceMockFactory.FailingRecaptchaService();
			
			var register = new RegisterModel
			{
				AcceptTos = true,
				Password = "hello1!...",
				ConfirmPassword = "hello1!...",
				Email = "no-reply@crossviewsoftware.io",
				RecaptchaToken = "abc123!.alkjsadfoijwe",
				UserName = "crossview1"
			};
			
			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.Register(register);
			
			Assert.NotNull(result);
			Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnauthorizedObjectResult)?.Value);
			
			var response = (result as UnauthorizedObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());
			var errors = response.Errors[nameof(register.RecaptchaToken)];

			Assert.Contains(nameof(register.RecaptchaToken), response.Errors.Keys);
			Assert.Equal(ErrorMessages.RecaptchaNoConnection, errors[0]);
		}

		[Fact]
		public async Task ReturnsUnprocessableEntityIfNotCreated()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService(userCreated: false);
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService();
			var recaptchaService = ServiceMockFactory.SucceedingRecaptchaService();
			
			var register = new RegisterModel
			{
				AcceptTos = true,
				Password = "hello1!...",
				ConfirmPassword = "hello1!...",
				Email = "no-reply@crossviewsoftware.io",
				RecaptchaToken = "abc123!.alkjsadfoijwe",
				UserName = "crossview1"
			};
			
			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.Register(register);
			
			Assert.NotNull(result);
			Assert.IsType<UnprocessableEntityObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnprocessableEntityObjectResult)?.Value);

			var response = (result as UnprocessableEntityObjectResult).Value as ApiResponse ?? new ApiResponse(new List<IdentityError>{ new IdentityErrorDescriber().DefaultError() });

			Assert.Contains("UserName", response.Errors.Keys);
			Assert.Single(response.Errors["UserName"]);
			Assert.Equal("User name 'crossview' is already taken.", response.Errors["UserName"][0]);
		}

		[Fact]
		public async Task ReturnsNewUserIfCreated()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService(EmailSenderService.SendWelcomeEmailSubject);
			var recaptchaService = ServiceMockFactory.SucceedingRecaptchaService();
			
			var register = new RegisterModel
			{
				AcceptTos = true,
				Password = "hello1!...",
				ConfirmPassword = "hello1!...",
				Email = "no-reply@crossviewsoftware.io",
				RecaptchaToken = "abc123!.alkjsadfoijwe",
				UserName = "crossview1"
			};
			
			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService);
			var result = await controller.Register(register);
				
			Assert.NotNull(result);
			Assert.IsType<CreatedResult>(result);
			Assert.IsType<ApiResponse<ApplicationUserModel>>((result as CreatedResult)?.Value);
		}
	}
}