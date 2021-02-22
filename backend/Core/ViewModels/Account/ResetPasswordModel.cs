using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.ViewModels.Account
{
	[ExcludeFromCodeCoverage]
	public class ResetPasswordModel
	{
		[Required]
		public string UserId { get; set; }

		[Required]
		public string VerificationCode { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required]
		[Compare("NewPassword", ErrorMessage = "The passwords do not match")]
		public string ConfirmNewPassword { get; set; }
	}
}