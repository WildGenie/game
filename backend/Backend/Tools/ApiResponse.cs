using System.Collections.Generic;
using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.Tools
{
	public class ApiResponse : ApiResponse<object>
	{
		public ApiResponse(IEnumerable<IdentityError> errors)
		{
			var emailErrorCodes = new List<string>
			{
				"DuplicateEmail",
				"InvalidEmail"
			};
			var usernameErrorCodes = new List<string>
			{
				"DuplicateUserName",
				"InvalidUserName"
			};
			var passwordErrorCodes = new List<string>
			{
				"PasswordMismatch",
				"PasswordRequiresDigit",
				"PasswordRequiresLower",
				"PasswordRequiresUpper",
				"PasswordTooShort",
				"PasswordRequiresNonAlphanumeric",
				"PasswordRequiresUniqueChars"
			};
			var roleNameErrorCodes = new List<string>
			{
				"DuplicateRoleName",
				"InvalidRoleName",
				"UserAlreadyInRole",
				"UserNotInRole"
			};
			Errors = new Dictionary<string, IList<string>>();

			foreach (var error in errors)
			{
				string errorName;
				if (emailErrorCodes.Contains(error.Code))
				{
					errorName = "Email";
				}
				else if (usernameErrorCodes.Contains(error.Code))
				{
					errorName = "UserName";
				}
				else if (passwordErrorCodes.Contains(error.Code))
				{
					errorName = "Password";
				}
				else if (roleNameErrorCodes.Contains(error.Code))
				{
					errorName = "RoleName";
				}
				else
				{
					errorName = "General";
				}

				if (!Errors.ContainsKey(errorName))
				{
					Errors[errorName] = new List<string>();
				}

				Errors[errorName]
					.Add(error.Description);
			}
		}

		public ApiResponse(ModelStateDictionary modelState)
		{
			Errors = new Dictionary<string, IList<string>>();
			foreach (var entry in modelState.Keys)
			{
				Errors.Add(entry, new List<string>());
				foreach (var error in modelState[entry]
					.Errors)
				{
					Errors[entry]
						.Add(error.ErrorMessage);
				}
			}
		}

		public ApiResponse(ServiceResult error)
		{
			Errors = new Dictionary<string, IList<string>>
			{
				{"General", new List<string> {error.Message}}
			};
		}
	}

	public class ApiResponse<TResult>
	{
		public TResult Result { get; set; }
		public IDictionary<string, IList<string>> Errors { get; set; }

		public ApiResponse(TResult result)
		{
			Result = result;
		}

		public ApiResponse() { }
	}
}