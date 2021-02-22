using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Core.Options;
using Microsoft.Extensions.Options;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;

namespace Tests.Mocks
{
	public static class EmailSenderServiceMocksFactory
	{
		public static Mock<ISendGridClient> SendGridClient(string subject, HttpStatusCode status = HttpStatusCode.Accepted, StringContent response = null)
		{
			response ??= new StringContent("{}");
			
			var client = new Mock<ISendGridClient>();
			client.Setup(c => c.SendEmailAsync(It.Is<SendGridMessage>(m => m.Subject.Equals(subject)), default))
				  .ReturnsAsync(() => new Response(status, response, It.IsAny<HttpResponseHeaders>()));

			return client;
		}
	}
}