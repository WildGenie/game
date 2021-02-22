using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.ViewModels.Account
{
	[ExcludeFromCodeCoverage]
	public class LoginModel
	{
		[Required]
		public string AccountName { get; set; }

		[Required]
		public string Password { get; set; }

		public bool RememberMe { get; set; }

		public string RecaptchaToken { get; set; }
	}
}