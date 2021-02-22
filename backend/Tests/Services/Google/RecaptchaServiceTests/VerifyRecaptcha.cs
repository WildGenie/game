using System.Net.Http;
using System.Threading.Tasks;
using Core;
using Moq;
using Services.Google;
using Tests.Mocks;
using Xunit;

namespace Tests.Services.Google.RecaptchaServiceTests
{
	public class VerifyRecaptcha
	{
		[Fact]
		public async Task ReturnsServiceResultOnSuccess()
		{
			var httpHandler = NetworkMockFactory.GeneralHandler(message: new StringContent("{\"success\":true}"));
			var httpClient = new HttpClient(httpHandler.Object);
			var logger = LoggerMockFactory.DefaultLogger();
			var config = ConfigurationMockFactory.RecaptchaSecretKeyConfiguration();
			
			var recaptcha = new RecaptchaService(httpClient, logger.Object, config.Object);
			var response = await recaptcha.VerifyRecaptcha("token");
			
			Assert.NotNull(response);
			Assert.True(response.WasSuccessful);
			Assert.Null(response.Message);
		}

		[Fact]
		public async Task ThrowsExceptionOnFailure()
		{
			var httpHandler = NetworkMockFactory.ThrowsHandler(new HttpRequestException());
			var httpClient = new HttpClient(httpHandler.Object);
			var logger = LoggerMockFactory.ErrorLoggerWithException<HttpRequestException>("Unable to contact reCAPTCHA service: {@exception}");
			var config = ConfigurationMockFactory.RecaptchaSecretKeyConfiguration();
			
			var recaptcha = new RecaptchaService(httpClient, logger.Object, config.Object);
			var result = await recaptcha.VerifyRecaptcha("token");

			Assert.False(result.WasSuccessful);
			Assert.Equal(ErrorMessages.RecaptchaNoConnection, result.Message);

			var errorLogged = false;
			try
			{
				logger.Verify();
				errorLogged = true;
			}
			catch (MockException) { }

			Assert.True(errorLogged);
		}

		[Fact]
		public async Task ReturnsErrorMessageOnFailedValidation()
		{
			var httpHandler = NetworkMockFactory.GeneralHandler(message: new StringContent("{\"success\":false,\"error-codes\":[\"missing-input-secret\"]}"));
			var httpClient = new HttpClient(httpHandler.Object);
			var logger = LoggerMockFactory.DefaultLogger();
			var config = ConfigurationMockFactory.RecaptchaSecretKeyConfiguration();
			
			var recaptcha = new RecaptchaService(httpClient, logger.Object, config.Object);
			var response = await recaptcha.VerifyRecaptcha("token");
			
			Assert.False(response.WasSuccessful);
			Assert.Equal("Application error", response.Message);
		}

		[Fact]
		public async Task ReturnsApplicationErrorMessageOnMissingInputSecret()
		{
			var httpHandler = NetworkMockFactory.GeneralHandler(message: new StringContent("{\"success\":false,\"error-codes\":[\"missing-input-secret\"]}"));
			var httpClient = new HttpClient(httpHandler.Object);
			var logger = LoggerMockFactory.ErrorLoggerWithSingleArg("reCAPTCHA reported application-level errors: {@errors}");
			var config = ConfigurationMockFactory.RecaptchaSecretKeyConfiguration();
			
			var recaptcha = new RecaptchaService(httpClient, logger.Object, config.Object);
			var response = await recaptcha.VerifyRecaptcha("token");
			var errorLogged = false;
			try
			{
				logger.Verify();
				errorLogged = true;
			}
			catch (MockException) { }

			Assert.Equal("Application error", response.Message);
			Assert.True(errorLogged);
		}
		
		[Fact]
		public async Task ReturnsApplicationErrorMessageOnInvalidInputSecret()
		{
			var httpHandler = NetworkMockFactory.GeneralHandler(message: new StringContent("{\"success\":false,\"error-codes\":[\"invalid-input-secret\"]}"));
			var httpClient = new HttpClient(httpHandler.Object);
			var logger = LoggerMockFactory.DefaultLogger();
			var config = ConfigurationMockFactory.RecaptchaSecretKeyConfiguration();
			
			var recaptcha = new RecaptchaService(httpClient, logger.Object, config.Object);
			var response = await recaptcha.VerifyRecaptcha("token");

			Assert.Equal("Application error", response.Message);
		}
		
		[Fact]
		public async Task ReturnsApplicationErrorMessageOnBadRequest()
		{
			var httpHandler = NetworkMockFactory.GeneralHandler(message: new StringContent("{\"success\":false,\"error-codes\":[\"bad-request\"]}"));
			var httpClient = new HttpClient(httpHandler.Object);
			var logger = LoggerMockFactory.DefaultLogger();
			var config = ConfigurationMockFactory.RecaptchaSecretKeyConfiguration();
			
			var recaptcha = new RecaptchaService(httpClient, logger.Object, config.Object);
			var response = await recaptcha.VerifyRecaptcha("token");

			Assert.Equal("Application error", response.Message);
		}
		
		[Fact]
		public async Task ReturnsRecaptchaTokenExpiredMessageOnTimeoutOrDuplicate()
		{
			var httpHandler = NetworkMockFactory.GeneralHandler(message: new StringContent("{\"success\":false,\"error-codes\":[\"timeout-or-duplicate\"]}"));
			var httpClient = new HttpClient(httpHandler.Object);
			var logger = LoggerMockFactory.ErrorLogger("reCAPTCHA reported a duplicate or timed-out response");
			var config = ConfigurationMockFactory.RecaptchaSecretKeyConfiguration();
			
			var recaptcha = new RecaptchaService(httpClient, logger.Object, config.Object);
			var response = await recaptcha.VerifyRecaptcha("token");
			var errorLogged = false;
			try
			{
				logger.Verify();
				errorLogged = true;
			}
			catch (MockException) { }

			Assert.Equal("reCAPTCHA token expired", response.Message);
			Assert.True(errorLogged);
		}
		
		[Fact]
		public async Task ReturnsRecaptchaTokenMissingMessageOnMissingInputResponse()
		{
			var httpHandler = NetworkMockFactory.GeneralHandler(message: new StringContent("{\"success\":false,\"error-codes\":[\"missing-input-response\"]}"));
			var httpClient = new HttpClient(httpHandler.Object);
			var logger = LoggerMockFactory.ErrorLogger("reCAPTCHA reported a missing user token");
			var config = ConfigurationMockFactory.RecaptchaSecretKeyConfiguration();
			
			var recaptcha = new RecaptchaService(httpClient, logger.Object, config.Object);
			var response = await recaptcha.VerifyRecaptcha("token");
			var errorLogged = false;
			try
			{
				logger.Verify();
				errorLogged = true;
			}
			catch (MockException) { }

			Assert.Equal("reCAPTCHA token missing", response.Message);
			Assert.True(errorLogged);
		}
		
		[Fact]
		public async Task ReturnsRecaptchaTokenInvalidOnInvalidInputResponse()
		{
			var httpHandler = NetworkMockFactory.GeneralHandler(message: new StringContent("{\"success\":false,\"error-codes\":[\"invalid-input-response\"]}"));
			var httpClient = new HttpClient(httpHandler.Object);
			var logger = LoggerMockFactory.ErrorLogger("reCAPTCHA reported an invalid user token");
			var config = ConfigurationMockFactory.RecaptchaSecretKeyConfiguration();
			
			var recaptcha = new RecaptchaService(httpClient, logger.Object, config.Object);
			var response = await recaptcha.VerifyRecaptcha("token");
			var errorLogged = false;
			try
			{
				logger.Verify();
				errorLogged = true;
			}
			catch (MockException) { }

			Assert.Equal("reCAPTCHA token invalid", response.Message);
			Assert.True(errorLogged);
		}
		
		[Fact]
		public async Task ReturnsUnknownErrorMessageOnUnanticipatedResponse()
		{
			var httpHandler = NetworkMockFactory.GeneralHandler(message: new StringContent("{\"success\":false,\"error-codes\":[\"unknown-error-code\"]}"));
			var httpClient = new HttpClient(httpHandler.Object);
			var logger = LoggerMockFactory.ErrorLoggerWithSingleArg("reCAPTCHA contains errors not currently accounted for by the application: {@errors}");
			var config = ConfigurationMockFactory.RecaptchaSecretKeyConfiguration();
			
			var recaptcha = new RecaptchaService(httpClient, logger.Object, config.Object);
			var response = await recaptcha.VerifyRecaptcha("token");
			var errorLogged = false;
			try
			{
				logger.Verify();
				errorLogged = true;
			}
			catch (MockException) { }

			Assert.Equal("Unknown reCAPTCHA error", response.Message);
			Assert.True(errorLogged);
		}
	}
}