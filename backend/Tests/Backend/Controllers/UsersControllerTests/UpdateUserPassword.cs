using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.UsersControllerTests
{
	public class UpdateUserPassword
	{
		[Fact]
		public async Task ReturnsNotFoundIfUserNotFound()
		{
			var userRepo = RepositoryMockFactory.UserRepository(userExists: false);
			var userService = ServiceMockFactory.UserService(userExists: false);
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();

			var userChangePasswordModel = new UserChangePasswordModel
			{
				UserId = "something unique",
				NewPassword = "5up3453cu43!",
				ConfirmNewPassword = "5up3453cu43!"
			};
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.UpdateUserPassword(userChangePasswordModel);
			
			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}
		
		[Fact]
		public async Task ReturnsNoContentIfSuccessful()
		{
			var userRepo = RepositoryMockFactory.UserRepository();
			var userService = ServiceMockFactory.UserService();
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();

			var userChangePasswordModel = new UserChangePasswordModel
			{
				UserId = "something unique",
				NewPassword = "5up3453cu43!",
				ConfirmNewPassword = "5up3453cu43!"
			};
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.UpdateUserPassword(userChangePasswordModel);
			
			Assert.NotNull(result);
			Assert.IsType<NoContentResult>(result);
		}
		
		[Fact]
		public async Task ReturnsUnprocessableEntityIfFails()
		{
			var userRepo = RepositoryMockFactory.UserRepository();
			var userService = ServiceMockFactory.UserService(newPasswordValid: false);
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();

			var userChangePasswordModel = new UserChangePasswordModel
			{
				UserId = "something unique",
				NewPassword = "5up3453cu43!",
				ConfirmNewPassword = "5up3453cu43!"
			};
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.UpdateUserPassword(userChangePasswordModel);
			
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