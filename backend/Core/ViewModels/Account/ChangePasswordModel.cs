using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.ViewModels.Account
{
	[ExcludeFromCodeCoverage]
	public class ChangePasswordModel
	{
		[Required]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required]
		[Compare("NewPassword", ErrorMessage = "The passwords do not match")]
		public string ConfirmNewPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string CurrentPassword { get; set; }
	}
}