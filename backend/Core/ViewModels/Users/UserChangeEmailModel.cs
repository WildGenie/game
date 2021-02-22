using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.ViewModels.Users
{
	[ExcludeFromCodeCoverage]
	public class UserChangeEmailModel
	{
		[Required]
		public string UserId { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Compare("Email", ErrorMessage = "The email addresses do not match")]
		public string ConfirmEmail { get; set; }
	}
}