using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests.Mocks
{
	public static class ConfigurationMockFactory
	{
		public static Mock<IConfiguration> RecaptchaSecretKeyConfiguration()
		{
			var config = new Mock<IConfiguration>();
			config
				.SetupGet(c => c[It.Is<string>(s => s == "Google:Recaptcha:secretKey")])
				.Returns("google-recaptcha-secret");

			return config;
		}

		public static Mock<IConfiguration> DatabaseSeedConfiguration()
		{
			var config = new Mock<IConfiguration>();
			config.SetupGet(c => c[It.Is<string>(s => s == "Application:Defaults:UserEmail")])
				  .Returns("no-reply@bastionofshadows.com");
			config.SetupGet(c => c[It.Is<string>(s => s == "Application:Defaults:UserName")])
				  .Returns("bastion");
			config.SetupGet(c => c[It.Is<string>(s => s == "Application:Defaults:UserPassword")])
				  .Returns("Sup345ecu4e!");

			return config;
		}
	}
}