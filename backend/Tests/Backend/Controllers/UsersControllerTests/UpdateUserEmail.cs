using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.UsersControllerTests
{
	public class UpdateUserEmail
	{
		[Fact]
		public async Task ReturnsNotFoundIfUserNotFound()
		{
			var userRepo = RepositoryMockFactory.UserRepository(userExists: false);
			var userService = ServiceMockFactory.UserService(userExists: false);
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();

			var userChangeEmailModel = new UserChangeEmailModel
			{
				UserId = "something unique",
				Email = "no-reply2@crossviewsoftware.io",
				ConfirmEmail = "no-reply2@crossviewsoftware.io"
			};
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.UpdateUserEmail(userChangeEmailModel);
			
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

			var userChangeEmailModel = new UserChangeEmailModel
			{
				UserId = "something unique",
				Email = "no-reply2@crossviewsoftware.io",
				ConfirmEmail = "no-reply2@crossviewsoftware.io"
			};
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.UpdateUserEmail(userChangeEmailModel);
			
			Assert.NotNull(result);
			Assert.IsType<NoContentResult>(result);
		}
	}
}