using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.AccountControllerTests
{
	public class CheckLogin
	{
		[Fact]
		public async Task ReturnsApiResponse()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService();
			var recaptchaService = ServiceMockFactory.RecaptchaService();
			
			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var result = await controller.CheckLogin();
			
			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
			Assert.IsType<ApiResponse<ApplicationUserModel>>((result as OkObjectResult)?.Value);
		}
		
		[Fact]
		public async Task ReturnsUserObject()
		{
			var logger = LoggerMockFactory.DefaultLogger();
			var options = OptionsMockFactory.ApiBehaviorOptions();
			var userService = ServiceMockFactory.UserService();
			var signInService = ServiceMockFactory.SignInService();
			var emailSenderService = ServiceMockFactory.EmailSenderService();
			var recaptchaService = ServiceMockFactory.RecaptchaService();
			
			var controller = new AccountController(logger.Object, options, userService.Object, signInService.Object, emailSenderService.Object, recaptchaService.Object);
			var response = await controller.CheckLogin() as OkObjectResult;
			var apiResponse = response?.Value as ApiResponse<ApplicationUserModel>;
			var user = apiResponse?.Result;
			
			Assert.NotNull(user);
			Assert.Equal("crossview", user.UserName);
			Assert.Equal("no-reply@crossviewsoftware.io", user.Email);
			Assert.Equal("something unique", user.Id);
		}
	}
}