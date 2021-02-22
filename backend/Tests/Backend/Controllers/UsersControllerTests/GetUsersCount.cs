using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.UsersControllerTests
{
	public class GetUsersCount
	{
		[Fact]
		public async Task ReturnsUserCount()
		{
			var userRepo = RepositoryMockFactory.UserRepository(userCount: 5);
			var userService = ServiceMockFactory.UserService();
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.GetUsersCount();
			
			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
			Assert.IsType<ApiResponse<UserCountModel>>((result as OkObjectResult)?.Value);

			var response = (result as OkObjectResult).Value as ApiResponse<UserCountModel> ?? new ApiResponse<UserCountModel>();
			
			Assert.Equal(5, response.Result.Count);
		}
	}
}