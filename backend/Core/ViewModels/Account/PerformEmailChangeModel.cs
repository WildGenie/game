using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.ViewModels.Account
{
	[ExcludeFromCodeCoverage]
	public class PerformEmailChangeModel
	{
		[Required]
		[EmailAddress]
		public string NewEmail { get; set; }

		[Required]
		public string UserId { get; set; }

		[Required]
		public string VerificationCode { get; set; }
	}
}