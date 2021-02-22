using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core;
using Microsoft.AspNetCore.Mvc;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.UsersControllerTests
{
	public class GetAvailableRoles
	{
		[Fact]
		public async Task ReturnsAvailableRoles()
		{
			var userRepo = RepositoryMockFactory.UserRepository();
			var userService = ServiceMockFactory.UserService();
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();
		
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.GetAvailableRoles();
		
			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
			Assert.IsType<ApiResponse<List<string>>>((result as OkObjectResult)?.Value);

			var response = (result as OkObjectResult).Value as ApiResponse<List<string>> ?? new ApiResponse<List<string>>();
			Assert.Single(response.Result);
			Assert.Contains(Roles.Admin, response.Result);
		}
	}
}