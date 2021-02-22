using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.ViewModels.Account
{
	[ExcludeFromCodeCoverage]
	public class InitiateEmailChangeModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[Compare("Email", ErrorMessage = "The email addresses do not match")]
		public string ConfirmEmail { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}