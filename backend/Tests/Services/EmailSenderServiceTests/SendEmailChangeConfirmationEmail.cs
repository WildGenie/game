using System.Threading.Tasks;
using Core;
using Moq;
using Services;
using Tests.Mocks;
using Xunit;

namespace Tests.Services.EmailSenderServiceTests
{
	public class SendEmailChangeConfirmationEmail
	{
		[Fact]
		public async Task SendsEmail()
		{
			const string username = "bastion";
			const string email = "no-reply@bastionofshadows.com";
			const string userId = "abc123";
			const string verificationCode = "xyz789";

			var logger = LoggerMockFactory.EmailSenderServiceInformationLogger(EmailSenderService.SendEmailChangeConfirmationEmailSubject);

			var options = OptionsMockFactory.EmailOptions();
			var emailMessages = new EmailMessages(options);

			var client = EmailSenderServiceMocksFactory.SendGridClient(EmailSenderService.SendEmailChangeConfirmationEmailSubject);

			var sender = new EmailSenderService(logger.Object, options, client.Object, emailMessages);
			await sender.SendEmailChangeConfirmationEmail(username, email, userId, verificationCode);

			var logWritten = false;
			try
			{
				logger.Verify();
				logWritten = true;
			}
			catch (MockException) { }

			Assert.True(logWritten);
		}
	}
}