using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.UsersControllerTests
{
	public class GetUserData
	{
		[Fact]
		public async Task ReturnsNotFoundIfUserNotFound()
		{
			var userRepo = RepositoryMockFactory.UserRepository(userExists: false);
			var userService = ServiceMockFactory.UserService(userExists: false);
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();

			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.GetUserData("something unique");
			
			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task ReturnsUserIfUserExists()
		{
			var userRepo = RepositoryMockFactory.UserRepository();
			var userService = ServiceMockFactory.UserService();
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.GetUserData("something unique");
			
			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
			Assert.IsType<ApiResponse<ApplicationUserModel>>((result as OkObjectResult)?.Value);

			var response = (result as OkObjectResult).Value as ApiResponse<ApplicationUserModel> ?? new ApiResponse<ApplicationUserModel>();
			var user = response.Result;
			
			Assert.Equal("something unique", user.Id);
			Assert.Equal("crossview", user.UserName);
			Assert.Equal("no-reply@crossviewsoftware.io", user.Email);
			Assert.True(user.IsVerified);
			Assert.Empty(user.Roles);
		}
	}
}