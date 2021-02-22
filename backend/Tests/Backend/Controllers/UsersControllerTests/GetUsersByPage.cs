using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core.ViewModels.Account;
using Core.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.UsersControllerTests
{
	public class GetUsersByPage
	{
		[Fact]
		public async Task ReturnsListOfUsers()
		{
			var userRepo = RepositoryMockFactory.UserRepository(userCount: 5);
			var userService = ServiceMockFactory.UserService();
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.GetUsersByPage();
			
			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
			Assert.IsType<ApiResponse<IEnumerable<ApplicationUserModel>>>((result as OkObjectResult)?.Value);

			var response = (result as OkObjectResult).Value as ApiResponse<IEnumerable<ApplicationUserModel>> ?? new ApiResponse<IEnumerable<ApplicationUserModel>>();

			Assert.Single(response.Result);
			Assert.Equal("something unique", response.Result.First().Id);
		}
	}
}