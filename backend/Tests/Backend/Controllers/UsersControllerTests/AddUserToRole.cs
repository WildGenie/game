using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core;
using Core.DataModels;
using Core.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.UsersControllerTests
{
	public class AddUserToRole
	{
		[Fact]
		public async Task ReturnsNotFoundIfUserNotFound()
		{
			var userRepo = RepositoryMockFactory.UserRepository(userExists: false);
			var userService = ServiceMockFactory.UserService(userExists: false);
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();

			var userAddToRoleModel = new UserAddToRoleModel
			{
				UserId = "something unique",
				Roles = new List<string> { Roles.Admin }
			};
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.AddUserToRole(userAddToRoleModel);
			
			Assert.NotNull(result);
			Assert.IsType<NotFoundObjectResult>(result);
			Assert.IsType<ApiResponse>((result as NotFoundObjectResult)?.Value);

			var response = (result as NotFoundObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			var name = nameof(userAddToRoleModel.UserId);
			Assert.Contains(name, response.Errors.Keys);
			Assert.Single(response.Errors[name]);
			Assert.Equal($"No user found with ID {userAddToRoleModel.UserId}", response.Errors[name][0]);
		}
		
		[Fact]
		public async Task ReturnsNotFoundIfRoleNotFound()
		{
			var userRepo = RepositoryMockFactory.UserRepository();
			var userService = ServiceMockFactory.UserService();
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager(roleExists: false);

			var userAddToRoleModel = new UserAddToRoleModel
			{
				UserId = "something unique",
				Roles = new List<string> { Roles.Admin }
			};
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.AddUserToRole(userAddToRoleModel);
			
			Assert.NotNull(result);
			Assert.IsType<NotFoundObjectResult>(result);
			Assert.IsType<ApiResponse>((result as NotFoundObjectResult)?.Value);

			var response = (result as NotFoundObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			var name = $"{Roles.Admin} Role";
			Assert.Contains(name, response.Errors.Keys);
			Assert.Single(response.Errors[name]);
			Assert.Equal($"No role found with name {Roles.Admin}", response.Errors[name][0]);
		}
		
		[Fact]
		public async Task ReturnsUnprocessableEntityIfUserNotRemovedFromRoles()
		{
			var userRepo = RepositoryMockFactory.UserRepository();
			var userService = ServiceMockFactory.UserService(removeFromRoleSuccessful: false);
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();

			var userAddToRoleModel = new UserAddToRoleModel
			{
				UserId = "something unique",
				Roles = new List<string> { Roles.Admin }
			};
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.AddUserToRole(userAddToRoleModel);
			
			Assert.NotNull(result);
			Assert.IsType<UnprocessableEntityObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnprocessableEntityObjectResult)?.Value);

			var response = (result as UnprocessableEntityObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			var name = "RoleName";
			Assert.Contains(name, response.Errors.Keys);
			Assert.Single(response.Errors[name]);
			Assert.Equal($"User is not in role '{Roles.Admin}'.", response.Errors[name][0]);
		}
		
		[Fact]
		public async Task ReturnsNoContentIfSuccessful()
		{
			var userRepo = RepositoryMockFactory.UserRepository();
			var userService = ServiceMockFactory.UserService();
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();

			var userAddToRoleModel = new UserAddToRoleModel
			{
				UserId = "something unique",
				Roles = new List<string> { Roles.Admin }
			};
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.AddUserToRole(userAddToRoleModel);
			
			Assert.NotNull(result);
			Assert.IsType<NoContentResult>(result);
		}
		
		[Fact]
		public async Task ReturnsUnprocessableEntityIfUserNotAddedToRoles()
		{
			var userRepo = RepositoryMockFactory.UserRepository();
			var userService = ServiceMockFactory.UserService(addToRoleSuccessful: false);
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();

			var userAddToRoleModel = new UserAddToRoleModel
			{
				UserId = "something unique",
				Roles = new List<string> { Roles.Admin }
			};
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.AddUserToRole(userAddToRoleModel);
			
			Assert.NotNull(result);
			Assert.IsType<UnprocessableEntityObjectResult>(result);
			Assert.IsType<ApiResponse>((result as UnprocessableEntityObjectResult)?.Value);

			var response = (result as UnprocessableEntityObjectResult).Value as ApiResponse ?? new ApiResponse(new ModelStateDictionary());

			var name = "RoleName";
			Assert.Contains(name, response.Errors.Keys);
			Assert.Single(response.Errors[name]);
			Assert.Equal($"User already in role '{Roles.Admin}'.", response.Errors[name][0]);
		}
		
		[Fact]
		public async Task ReturnsNoContentIfAllRolesExistInModelRoles()
		{
			var userRepo = RepositoryMockFactory.UserRepository();
			var userService = ServiceMockFactory.UserService();
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();

			var userAddToRoleModel = new UserAddToRoleModel
			{
				UserId = "something unique",
				Roles = new List<string> { Roles.Admin }
			};
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.AddUserToRole(userAddToRoleModel);
			
			Assert.NotNull(result);
			Assert.IsType<NoContentResult>(result);
		}
		
		[Fact]
		public async Task ReturnsNoContentIfNoRolesPassed()
		{
			var userRepo = RepositoryMockFactory.UserRepository();
			var userService = ServiceMockFactory.UserService();
			var emailService = ServiceMockFactory.EmailSenderService();
			var roleManager = ServiceMockFactory.RoleManager();

			var userAddToRoleModel = new UserAddToRoleModel
			{
				UserId = "something unique",
				Roles = new List<string> {  }
			};
			
			var controller = new UsersController(userRepo.Object, userService.Object, emailService.Object, roleManager.Object);
			var result = await controller.AddUserToRole(userAddToRoleModel);
			
			Assert.NotNull(result);
			Assert.IsType<NoContentResult>(result);
		}
	}
}