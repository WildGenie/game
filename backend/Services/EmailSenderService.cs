using System.Threading.Tasks;
using Core;
using Core.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;

namespace Services
{
	public class EmailSenderService : IEmailSenderService
	{
		public const string SendWelcomeEmailSubject = "Welcome to <Service Name>";
		public const string SendEmailChangeConfirmationEmailSubject = "Confirm Your New Email";
		public const string SendPasswordResetEmailSubject = "Password Reset Confirmation";
		
		private readonly ILogger _logger;
		private readonly IOptions<EmailOptions> _options;
		private readonly ISendGridClient _client;
		private readonly EmailAddress _from;
		private readonly EmailMessages _emailMessages;

		public EmailSenderService(ILogger logger, IOptions<EmailOptions> options, ISendGridClient client, EmailMessages emailMessages)
		{
			_logger = logger;
			_options = options;
			_client = client;
			_from = new EmailAddress(_options.Value.EmailFromAddress, _options.Value.EmailFromName);
			_emailMessages = emailMessages;

			_logger.Information($"Created EmailSenderService instance");
		}

		public async Task SendWelcomeEmail(string username, string email, string userId, string verificationCode)
		{
			var message = new SendGridMessage
			{
				From = new EmailAddress(_options.Value.EmailFromAddress, _options.Value.EmailFromName),
				Subject = SendWelcomeEmailSubject,
				PlainTextContent = _emailMessages.WelcomeEmailText(username, userId, verificationCode),
				HtmlContent = _emailMessages.WelcomeEmailHtml(username, userId, verificationCode)
			};
			message.AddTo(email, username);

			await SendEmail(message);
		}

		public async Task SendEmailChangeConfirmationEmail(string username, string email, string userId, string verificationCode)
		{
			var message = new SendGridMessage
			{
				From = new EmailAddress(_options.Value.EmailFromAddress, _options.Value.EmailFromName),
				Subject = SendEmailChangeConfirmationEmailSubject,
				PlainTextContent = _emailMessages.ChangeEmailText(username, email, userId, verificationCode),
				HtmlContent = _emailMessages.ChangeEmailHtml(username, email, userId, verificationCode)
			};
			message.AddTo(email, username);

			await SendEmail(message);
		}

		public async Task SendPasswordResetEmail(string username, string email, string userId, string verificationCode)
		{
			var message = new SendGridMessage
			{
				Subject = SendPasswordResetEmailSubject,
				PlainTextContent = _emailMessages.ResetPasswordText(username, userId, verificationCode),
				HtmlContent = _emailMessages.ResetPasswordHtml(username, userId, verificationCode)
			};
			message.AddTo(email, username);

			await SendEmail(message);
		}

		private async Task SendEmail(SendGridMessage email)
		{
			email.From ??= _from;

			var response = await _client.SendEmailAsync(email);

			_logger.Information("Email sent: {subject}, {status}, {response}", email.Subject, response.StatusCode, await response.Body.ReadAsStringAsync());

			if ((int) response.StatusCode > 299)
			{
				_logger.Error("Email failed to send: {status}, {response}", response.StatusCode, await response.Body.ReadAsStringAsync());
			}
		}
	}
}