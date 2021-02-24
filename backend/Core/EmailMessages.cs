using Core.Options;
using Microsoft.Extensions.Options;

namespace Core
{
	public class EmailMessages
	{
		private const string ConfirmationUrl = "/users/confirm";
		private const string EmailChangeUrl = "/account/email";
		private const string ResetPasswordUrl = "/users/reset-password";
		private const string TeamName = "The Website Team";
		private readonly string _applicationUrl;

		public EmailMessages(IOptions<EmailOptions> options)
		{
			_applicationUrl = options.Value.ApplicationUrl;
		}

		public string WelcomeEmailHtml(string username, string userId, string verificationCode)
		{
			return $"<!DOCTYPE html><html><head><title>Welcome,{username}!</title></head><body><p>Hello {username},</p><p>Thank you for registering. Before you can sign in, you need to confirm your account by clicking the following link: <a href='{_applicationUrl}{ConfirmationUrl}/{userId}/{verificationCode}'>confirm account</a></p><p>Regards,</p><p>{TeamName}</p></body></html>";
		}

		public string WelcomeEmailText(string username, string userId, string verificationCode)
		{
			return $"Hello {username},\n\n" +
				   $"Thank you for registering. Before you can sign in, you need to confirm your account by copying the following link into your browser: {_applicationUrl}{ConfirmationUrl}/{userId}/{verificationCode}\n\n" +
				   "Regards,\n\n" +
				   TeamName;
		}

		public string ChangeEmailHtml(string username, string newEmail, string userId, string verificationCode)
		{
			return $"<!DOCTYPE html><html><head><title>Confirm your new email address, {username}!</title></head><body><p>Hello {username},</p><p>Before you can sign in using your new email address, you need to confirm it by clicking the following link: <a href='{_applicationUrl}{EmailChangeUrl}/{newEmail}/{userId}/{verificationCode}'>confirm account</a></p><p>Regards,</p><p>{TeamName}</p></body></html>";
		}

		public string ChangeEmailText(string username, string newEmail, string userId, string verificationCode)
		{
			return $"Hello {username},\n\n" +
				   $"Before you can sign in using your new email address, you need to confirm it by copying the following link into your browser: {_applicationUrl}{EmailChangeUrl}/{newEmail}/{userId}/{verificationCode}\n\n" +
				   "Regards,\n\n" +
				   TeamName;
		}

		public string ResetPasswordHtml(string username, string userId, string verificationCode)
		{
			return $"<!DOCTYPE html><html><head><title>Password Reset Request</title></head><body><p>Hello {username},</p><p>We received a request to reset your password. If this was you, you can reset your password by clicking the following link: <a href='{_applicationUrl}{ResetPasswordUrl}/{userId}/{verificationCode}'>confirm account</a></p><p>If this was not you, delete this email. The reset code will expire in 30 minutes and your account details will not be changed.</p><p>Regards,</p><p>{TeamName}</p></body></html>";
		}

		public string ResetPasswordText(string username, string userId, string verificationCode)
		{
			return $"Hello {username},\n\n" +
				   $"We received a request to reset your password. If this was you, you can reset your password by copying the following link into your browser: {_applicationUrl}{ResetPasswordUrl}/{userId}/{verificationCode}\n\n" +
				   "If this was not you, delete this email. The reset code will expire in 30 minutes and your account details will not be changed.\n\n" +
				   "Regards,\n\n" +
				   TeamName;
		}
	}
}