using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.ViewModels.Account
{
	[ExcludeFromCodeCoverage]
	public class DeleteAccountModel
	{
		[Required]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}