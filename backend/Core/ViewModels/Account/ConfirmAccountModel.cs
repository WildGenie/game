using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.ViewModels.Account
{
	[ExcludeFromCodeCoverage]
	public class ConfirmAccountModel
	{
		[Required]
		public string UserId { get; set; }

		[Required]
		public string VerificationCode { get; set; }
	}
}