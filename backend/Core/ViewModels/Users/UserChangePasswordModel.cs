using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.ViewModels.Users
{
	[ExcludeFromCodeCoverage]
	public class UserChangePasswordModel
	{
		[Required]
		public string UserId { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required]
		[Compare("NewPassword", ErrorMessage = "The passwords do not match")]
		public string ConfirmNewPassword { get; set; }
	}
}