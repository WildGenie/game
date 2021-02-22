using System;
using System.Net;
using Core.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ILogger = Serilog.ILogger;

namespace Tests.Mocks
{
	public static class LoggerMockFactory
	{
		// General mocks
		public static Mock<ILogger> DefaultLogger()
		{
			return new Mock<ILogger>();
		}
		
		public static Mock<ILogger> InformationLogger(string message)
		{
			var logger = new Mock<ILogger>();
			logger.Setup(l => l.Information(message))
				  .Verifiable();

			return logger;
		}

		public static Mock<ILogger> InformationLoggerWithThreeArgs(string message)
		{
			var logger = new Mock<ILogger>();
			logger.Setup(l => l.Information(message, It.IsAny<It.IsAnyType>(), It.IsAny<It.IsAnyType>(), It.IsAny<It.IsAnyType>()))
				  .Verifiable();

			return logger;
		}

		public static Mock<ILogger> ErrorLogger(string message)
		{
			var logger = new Mock<ILogger>();
			logger.Setup(l => l.Error(message))
				  .Verifiable();

			return logger;
		}

		public static Mock<ILogger> ErrorLoggerWithSingleArg(string message)
		{
			var logger = new Mock<ILogger>();
			logger.Setup(l => l.Error(message, It.IsAny<It.IsAnyType>()))
				  .Verifiable();

			return logger;
		}

		public static Mock<ILogger> ErrorLoggerWithException<T>(string message) where T : Exception
		{
			var logger = new Mock<ILogger>();
			logger.Setup(l => l.Error(message, It.IsAny<T>()))
				  .Verifiable();

			return logger;
		}
		
		// EmailSenderService mocks

		public static Mock<ILogger> EmailSenderServiceInformationLogger(string subject, HttpStatusCode status = HttpStatusCode.Accepted, string response = "{}")
		{
			var logger = new Mock<ILogger>();
			logger.Setup(l => l.Information("Email sent: {subject}, {status}, {response}", subject, status, response))
				  .Verifiable();

			return logger;
		}
		
		public static Mock<ILogger> EmailSenderServiceErrorLogger(HttpStatusCode status = HttpStatusCode.Accepted, string response = "{}")
		{
			var logger = new Mock<ILogger>();
			logger.Setup(l => l.Error("Email failed to send: {status}, {response}", status, response))
				  .Verifiable();

			return logger;
		}

		public static Mock<ILogger<UserManager<ApplicationUser>>> UserManagerLogger()
		{
			return new Mock<ILogger<UserManager<ApplicationUser>>>();
		}

		public static Mock<ILogger<SignInManager<ApplicationUser>>> SignInManagerLogger()
		{
			return new Mock<ILogger<SignInManager<ApplicationUser>>>();
		}

		public static Mock<ILogger<RoleManager<Role>>> RoleManagerLogger()
		{
			return new Mock<ILogger<RoleManager<Role>>>();
		}
	}
}