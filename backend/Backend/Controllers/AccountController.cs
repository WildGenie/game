using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.Tools;
using Core;
using Core.DataModels;
using Core.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Serilog;
using Services;
using Services.Google;
using Services.Identity;

namespace Backend.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;
		private readonly UserService _userService;
		private readonly SignInService _signInService;
		private readonly IEmailSenderService _emailService;
		private readonly RecaptchaService _recaptchaService;

		public AccountController(ILogger logger, IOptions<ApiBehaviorOptions> apiBehaviorOptions, UserService userService, SignInService signInService, IEmailSenderService emailService, RecaptchaService recaptchaService)
		{
			_logger = logger;
			_apiBehaviorOptions = apiBehaviorOptions;
			_userService = userService;
			_signInService = signInService;
			_emailService = emailService;
			_recaptchaService = recaptchaService;
		}

		[HttpGet]
		public async Task<IActionResult> CheckLogin()
		{
			var user = await _userService.GetUserAsync(User);
			return Ok(new ApiResponse<ApplicationUserModel>(new ApplicationUserModel(user)));
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel userData)
		{
			// User must accept the Terms of Service
			if (!userData.AcceptTos)
			{
				ModelState.AddModelError(nameof(userData.AcceptTos), "You must accept the Terms of Service in order to register");
				return GetInvalidModelStateResponse();
			}

			var recaptchaResponse = await _recaptchaService.VerifyRecaptcha(userData.RecaptchaToken);
			if (!recaptchaResponse.WasSuccessful)
			{
				ModelState.AddModelError(nameof(userData.RecaptchaToken), recaptchaResponse.Message);
				return Unauthorized(new ApiResponse(ModelState));
			}

			// Make a new user
			var user = new ApplicationUser
			{
				UserName = userData.UserName,
				Email = userData.Email
			};

			// Try to create that user with the given password
			var result = await _userService.CreateAsync(user, userData.Password);

			// If successful, send them their new account data
			if (result.Succeeded)
			{
				await SendWelcomeEmail(user);
				return Created(nameof(Register), new ApiResponse<ApplicationUserModel>(new ApplicationUserModel(user)));
			}

			// If not, tell them why
			return UnprocessableEntity(new ApiResponse(result.Errors));
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginModel login)
		{
			// Check if the account name exists
			// If not, check if the user used their email address
			var user = await _userService.FindByNameAsync(login.AccountName);
			user ??= await _userService.FindByEmailAsync(login.AccountName);

			// If still not, return not found
			// Some say this is bad practice
			// but user enumeration is dead simple anyway
			// so who is it really impeding? no one
			if (user == null)
			{
				ModelState.AddModelError(nameof(login.AccountName), "An account with that name was not found.");
				return NotFound(new ApiResponse(ModelState));
			}

			// If user is locked out, tell them when the lockout date ends
			// 
			if (await _userService.IsLockedOutAsync(user))
			{
				ModelState.AddModelError("", $"The specified user account is currently locked out. The lockout will end {GetLockoutEndDateString(user.LockoutEnd ?? DateTimeOffset.MaxValue) ?? "never"}");
				return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse(ModelState));
			}

			if (user.AccessFailedCount > 2)
			{
				var recaptchaResponse = await _recaptchaService.VerifyRecaptcha(login.RecaptchaToken);
				if (!recaptchaResponse.WasSuccessful)
				{
					ModelState.AddModelError(nameof(login.RecaptchaToken), recaptchaResponse.Message);
					return Unauthorized(new ApiResponse(ModelState));
				}
			}

			login.AccountName = user.UserName;

			// If a user exists but isn't confirmed
			// check their password. If it matches,
			// resend their verification email
			if (!user.EmailConfirmed)
			{
				if (await _userService.CheckPasswordAsync(user, login.Password))
				{
					await SendWelcomeEmail(user);
					ModelState.AddModelError("", ErrorMessages.LoginFailedNotConfirmed);
					return Unauthorized(new ApiResponse(ModelState));
				}
			}

			var result = await _signInService.PasswordSignInAsync(login.AccountName, login.Password, login.RememberMe, true);

			if (result.Succeeded)
			{
				return Ok(new ApiResponse<ApplicationUserModel>(new ApplicationUserModel(user)));
			}

			if (result.IsLockedOut)
			{
				ModelState.AddModelError("", ErrorMessages.LoginFailedLocked);
				return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse(ModelState));
			}

			ModelState.AddModelError("", result.RequiresTwoFactor ? ErrorMessages.LoginFailed2fa : ErrorMessages.LoginFailedInvalid);

			if (user.AccessFailedCount >= 2)
			{
				ModelState.AddModelError(nameof(login.RecaptchaToken), $"Please complete the reCAPTCHA challenge as an additional security measure");
			}

			return Unauthorized(new ApiResponse(ModelState));
		}

		[HttpDelete("login")]
		public async Task<IActionResult> Logout()
		{
			await _signInService.SignOutAsync();
			return NoContent();
		}

		[AllowAnonymous]
		[HttpPost("confirm")]
		public async Task<IActionResult> ConfirmAccount(ConfirmAccountModel accountModel)
		{
			var user = await _userService.FindByIdAsync(accountModel.UserId);
			if (user == null)
			{
				return NotFound(new ApiResponse(ModelState));
			}

			accountModel.VerificationCode = Base64Decode(accountModel.VerificationCode);

			var result = await _userService.ConfirmEmailAsync(user, accountModel.VerificationCode);

			if (result.Succeeded)
			{
				return NoContent();
			}

			return UnprocessableEntity(new ApiResponse(result.Errors));
		}

		[HttpPost("email")]
		public async Task<IActionResult> InitiateEmailChange(InitiateEmailChangeModel emailModel)
		{
			var user = await _userService.GetUserAsync(User);
			if (!await _userService.CheckPasswordAsync(user, emailModel.ConfirmPassword))
			{
				return Unauthorized();
			}

			user.PendingEmail = emailModel.Email;
			await _userService.UpdateAsync(user);

			await SendEmailChangeConfirmationEmail(user);

			return NoContent();
		}

		[HttpPatch("email")]
		public async Task<IActionResult> PerformEmailChange(PerformEmailChangeModel changeModel)
		{
			_logger.Information("Performing email change for user {userId} to {newEmail} with token {verificationCode}", changeModel.UserId, changeModel.NewEmail, changeModel.VerificationCode);

			var user = await _userService.GetUserAsync(User);
			if (user.PendingEmail != changeModel.NewEmail)
			{
				ModelState.AddModelError("", ErrorMessages.EmailErrorWrongEmail);
				return Conflict(new ApiResponse(ModelState));
			}

			if (user.Id != changeModel.UserId)
			{
				ModelState.AddModelError("", ErrorMessages.AccountErrorWrongId);
				return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse(ModelState));
			}

			changeModel.VerificationCode = Base64Decode(changeModel.VerificationCode);
			var result = await _userService.ChangeEmailAsync(user, user.PendingEmail, changeModel.VerificationCode);

			if (result.Succeeded)
			{
				return NoContent();
			}

			return UnprocessableEntity(new ApiResponse(result.Errors));
		}

		[HttpPost("password")]
		public async Task<IActionResult> ChangePassword(ChangePasswordModel passwordModel)
		{
			var user = await _userService.GetUserAsync(User);
			var result = await _userService.ChangePasswordAsync(user, passwordModel.CurrentPassword, passwordModel.NewPassword);
			if (result.Succeeded)
			{
				return NoContent();
			}

			return UnprocessableEntity(new ApiResponse(result.Errors));
		}

		[AllowAnonymous]
		[HttpDelete("password")]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordModel passwordModel)
		{
			var user = await _userService.FindByNameAsync(passwordModel.AccountName);
			user ??= await _userService.FindByEmailAsync(passwordModel.AccountName);

			// They don't need to know whether the user exists or not
			// This ISN'T to prevent a general enumeration attack
			// Instead, this adds guesswork to a targeted attack against a specific user
			if (user == null)
			{
				return Accepted();
			}

			var recaptchaResponse = await _recaptchaService.VerifyRecaptcha(passwordModel.RecaptchaToken);
			if (!recaptchaResponse.WasSuccessful)
			{
				ModelState.AddModelError(nameof(passwordModel.RecaptchaToken), recaptchaResponse.Message);
				return Unauthorized(new ApiResponse(ModelState));
			}

			await SendPasswordResetEmail(user);

			return Accepted();
		}

		[AllowAnonymous]
		[HttpPatch("password")]
		public async Task<IActionResult> ResetPassword(ResetPasswordModel passwordModel)
		{
			var user = await _userService.FindByIdAsync(passwordModel.UserId);
			if (user == null)
			{
				return NotFound();
			}

			passwordModel.VerificationCode = Base64Decode(passwordModel.VerificationCode);

			var result = await _userService.ResetPasswordAsync(user, passwordModel.VerificationCode, passwordModel.NewPassword);
			if (result.Succeeded)
			{
				return NoContent();
			}

			return UnprocessableEntity(new ApiResponse(result.Errors));
		}

		[HttpGet("data")]
		public async Task<IActionResult> GetPersonalData()
		{
			var user = await _userService.GetUserAsync(User);

			// Only include personal data for download
			var personalData = new Dictionary<string, string>();
			var personalDataProps = user.GetType()
										.GetProperties()
										.Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
			foreach (var p in personalDataProps)
			{
				personalData.Add(p.Name, p.GetValue(user)
										  ?.ToString() ?? "null");
			}

			var logins = await _userService.GetLoginsAsync(user);
			foreach (var l in logins)
			{
				personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
			}

			personalData.Add("Authenticator Key", await _userService.GetAuthenticatorKeyAsync(user));

			Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
			return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteAccount(DeleteAccountModel model)
		{
			var user = await _userService.GetUserAsync(User);
			if (!await _userService.CheckPasswordAsync(user, model.ConfirmPassword))
			{
				ModelState.AddModelError("", ErrorMessages.LoginFailedInvalid);
				return Unauthorized(new ApiResponse(ModelState));
			}

			var result = await _userService.DeleteAsync(user);
			if (result.Succeeded)
			{
				await _signInService.SignOutAsync();
				return NoContent();
			}

			return UnprocessableEntity(new ApiResponse(result.Errors));
		}

		private IActionResult GetInvalidModelStateResponse()
		{
			return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
		}

		private async Task SendWelcomeEmail(ApplicationUser user)
		{
			var code = await _userService.GenerateEmailConfirmationTokenAsync(user);
			await _emailService.SendWelcomeEmail(user.UserName, user.Email, user.Id, Base64Encode(code));
		}

		private async Task SendEmailChangeConfirmationEmail(ApplicationUser user)
		{
			var code = await _userService.GenerateChangeEmailTokenAsync(user, user.PendingEmail);
			await _emailService.SendEmailChangeConfirmationEmail(user.UserName, user.PendingEmail, user.Id, Base64Encode(code));
		}

		private async Task SendPasswordResetEmail(ApplicationUser user)
		{
			var code = await _userService.GeneratePasswordResetTokenAsync(user);
			await _emailService.SendPasswordResetEmail(user.UserName, user.Email, user.Id, Base64Encode(code));
		}

		private string Base64Encode(string input)
		{
			return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(input));
		}

		private string Base64Decode(string input)
		{
			return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input));
		}

		private string GetLockoutEndDateString(DateTimeOffset endDate)
		{
			if (endDate == DateTimeOffset.MaxValue)
				return null;

			return $"{endDate:D} at {endDate:h:mm:ss tt} UTC";
		}
	}
}