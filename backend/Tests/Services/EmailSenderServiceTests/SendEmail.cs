using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Core;
using Moq;
using Services;
using Tests.Mocks;
using Xunit;

namespace Tests.Services.EmailSenderServiceTests
{
	public class SendEmail
	{
		[Fact]
		public async Task LogsErrorOnFailedSend()
		{
			const string username = "bastion";
			const string email = "no-reply@bastionofshadows.com";
			const string userId = "abc123";
			const string verificationCode = "xyz789";
			const string response = "{\"error\": \"Invalid format\"}";

			var logger = LoggerMockFactory.EmailSenderServiceErrorLogger(HttpStatusCode.BadRequest, response);

			var options = OptionsMockFactory.EmailOptions();
			var emailMessages = new EmailMessages(options);

			var client = EmailSenderServiceMocksFactory.SendGridClient(EmailSenderService.SendWelcomeEmailSubject, HttpStatusCode.BadRequest, new StringContent(response));

			var sender = new EmailSenderService(logger.Object, options, client.Object, emailMessages);
			await sender.SendWelcomeEmail(username, email, userId, verificationCode);

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