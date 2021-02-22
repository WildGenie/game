using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.ViewModels.Account
{
	[ExcludeFromCodeCoverage]
	public class ForgotPasswordModel
	{
		[Required]
		public string AccountName { get; set; }

		[Required]
		public string RecaptchaToken { get; set; }
	}
}