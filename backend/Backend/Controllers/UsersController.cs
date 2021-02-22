using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Tools;
using Core;
using Core.DataModels;
using Core.ViewModels.Account;
using Core.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using Services.Identity;

namespace Backend.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize(Roles = Roles.Admin)]
	public class UsersController : ControllerBase
	{
		private readonly IUserRepository _userRepository;
		private readonly UserService _userService;
		private readonly IEmailSenderService _emailService;
		private readonly RoleManager<Role> _roleManager;

		public UsersController(IUserRepository userRepository, UserService userService, IEmailSenderService emailService, RoleManager<Role> roleManager)
		{
			_userRepository = userRepository;
			_userService = userService;
			_emailService = emailService;
			_roleManager = roleManager;
		}

		[HttpGet]
		public async Task<IActionResult> GetUserData(string userId)
		{
			var user = await _userRepository.FindById(userId);
			if (user == null)
			{
				return NotFound();
			}

			return Ok(new ApiResponse<ApplicationUserModel>(new ApplicationUserModel(user)));
		}

		[HttpGet("count")]
		public async Task<IActionResult> GetUsersCount()
		{
			var userCount = new UserCountModel
			{
				Count = await _userRepository.GetCount()
			};

			return Ok(new ApiResponse<UserCountModel>(userCount));
		}

		[HttpGet("pagified")]
		public async Task<IActionResult> GetUsersByPage(int page = 1, int resultsPerPage = 10)
		{
			var users = await _userRepository.GetPagified(page, resultsPerPage);
			var userViewModels = users.Select(u => new ApplicationUserModel(u));

			return Ok(new ApiResponse<IEnumerable<ApplicationUserModel>>(userViewModels));
		}

		[HttpPatch("email")]
		public async Task<IActionResult> UpdateUserEmail(UserChangeEmailModel model)
		{
			// Find user
			var user = await _userService.FindByIdAsync(model.UserId);
			if (user == null)
			{
				return NotFound();
			}

			// Add pending email
			user.PendingEmail = model.Email;
			await _userService.UpdateAsync(user);

			// Generate verification code
			var code = await _userService.GenerateChangeEmailTokenAsync(user, user.PendingEmail);
			var codeBytes = Encoding.UTF8.GetBytes(code);
			code = WebEncoders.Base64UrlEncode(codeBytes);

			// Send verification code
			await _emailService.SendEmailChangeConfirmationEmail(user.UserName, user.PendingEmail, user.Id, code);

			return NoContent();
		}

		[HttpPatch("password")]
		public async Task<IActionResult> UpdateUserPassword(UserChangePasswordModel model)
		{
			var user = await _userService.FindByIdAsync(model.UserId);
			if (user == null)
			{
				return NotFound();
			}

			var result = await _userService.ChangePasswordAsync(user, model.NewPassword);
			if (result.Succeeded)
			{
				return NoContent();
			}

			return UnprocessableEntity(new ApiResponse(result.Errors));
		}

		[HttpGet("roles")]
		public async Task<IActionResult> GetAvailableRoles()
		{
			var roles = await _roleManager.Roles.Select(r => r.Name)
										  .ToListAsync();
			return Ok(new ApiResponse<List<string>>(roles));
		}

		[HttpPost("roles")]
		public async Task<IActionResult> AddUserToRole(UserAddToRoleModel model)
		{
			var user = await _userService.FindByIdAsync(model.UserId);
			if (user == null)
			{
				ModelState.AddModelError(nameof(model.UserId), $"No user found with ID {model.UserId}");
				return NotFound(new ApiResponse(ModelState));
			}

			foreach (var roleName in model.Roles)
			{
				var role = await _roleManager.FindByNameAsync(roleName);
				if (role == null)
				{
					// This key ensures that this error will be added to the errors.unknown frontend array
					// Otherwise, every checkbox will be highlighted red in the form, which makes no sense
					ModelState.AddModelError($"{roleName} Role", $"No role found with name {roleName}");
					return NotFound(new ApiResponse(ModelState));
				}
			}

			var userRoles = user.Roles.Select(r => r.Name);
			var removeFromRolesResult = await _userService.RemoveFromRolesAsync(user, userRoles);
			if (!removeFromRolesResult.Succeeded)
			{
				return UnprocessableEntity(new ApiResponse(removeFromRolesResult.Errors));
			}

			var result = await _userService.AddToRolesAsync(user, model.Roles);
			if (result.Succeeded)
			{
				return NoContent();
			}

			return UnprocessableEntity(new ApiResponse(result.Errors));
		}
	}
}