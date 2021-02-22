using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.ViewModels.Account
{
	[ExcludeFromCodeCoverage]
	public class RegisterModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(32, ErrorMessage = "Username must be between 6 and 32 characters", MinimumLength = 6)]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[StringLength(64, ErrorMessage = "Password must be between 8 and 64 characters", MinimumLength = 8)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The passwords do not match")]
		public string ConfirmPassword { get; set; }

		[Required]
		public string RecaptchaToken { get; set; }

		public bool AcceptTos { get; set; }
	}
}