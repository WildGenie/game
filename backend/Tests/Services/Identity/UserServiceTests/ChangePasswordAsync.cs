using System.Threading.Tasks;
using Core.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Identity;
using Tests.Mocks;
using Xunit;

namespace Tests.Services.Identity.UserServiceTests
{
	public class ChangePasswordAsync
	{
		[Fact]
		public async Task UpdatesUser()
		{
			var passwordHasher = IdentityMockFactory.PasswordHasher();
			var store = IdentityMockFactory.UserStore();
			var options = Options.Create(new IdentityOptions());
			var userValidators = IdentityMockFactory.UserValidators();
			var passwordValidators = IdentityMockFactory.PasswordValidators(true);
			var keyNormalizer = IdentityMockFactory.LookupNormalizer();
			var errors = IdentityMockFactory.ErrorDescriber();
			var services = AspNetMockFactory.ServiceProvider();
			var logger = LoggerMockFactory.UserManagerLogger();
			var service = new UserService(store.Object, options, passwordHasher.Object, userValidators, passwordValidators, keyNormalizer.Object, errors.Object, services.Object, logger.Object);
			var result = await service.ChangePasswordAsync(new ApplicationUser(), "some password");

			Assert.Empty(result.Errors);
			Assert.True(result.Succeeded);
		}
	}
}