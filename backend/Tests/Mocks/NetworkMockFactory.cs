using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace Tests.Mocks
{
	public static class NetworkMockFactory
	{
		public static Mock<HttpMessageHandler> GeneralHandler(HttpStatusCode status = HttpStatusCode.OK, HttpContent message = null)
		{
			message ??= new StringContent("{}");

			var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
			handler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = status,
					Content = message
				})
				.Verifiable();

			return handler;
		}

		public static Mock<HttpMessageHandler> ThrowsHandler<T>(T e, HttpStatusCode statusCode = HttpStatusCode.BadRequest, HttpContent message = null) where T : Exception
		{
			message ??= new StringContent("{}");

			var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
			handler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.Throws(e)
				.Verifiable();

			return handler;
		}

		public static Mock<HttpClient> GeneralHttpClient(HttpMessageHandler handler = null)
		{
			handler ??= GeneralHandler()
				.Object;
			
			return new Mock<HttpClient>(handler);
		}
	}
}