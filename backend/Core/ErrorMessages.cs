using System.Diagnostics.CodeAnalysis;

namespace Core
{
	[ExcludeFromCodeCoverage]
	public class ErrorMessages
	{
		public const string LoginFailed2fa = "You must log in using 2-factor authentication.";
		public const string LoginFailedInvalid = "Invalid credentials supplied.";
		public const string LoginFailedLocked = "Your account has been locked due to excessive login attempts. Please wait 15 minutes and try again.";
		public const string LoginFailedNotConfirmed = "You have not confirmed your email address. Please check your email for a confirmation link and click it to confirm your email address.";

		public const string EmailErrorWrongEmail = "That email does not match the email change request you initiated.";

		public const string AccountErrorWrongId = "The User ID supplied does not match your account.";

		// ReCAPTCHA
		public const string RecaptchaNoConnection = "The server was unable to connect to the Google reCAPTCHA service";
	}
}