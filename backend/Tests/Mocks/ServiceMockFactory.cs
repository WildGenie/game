using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using Core;
using Core.DataModels;
using Core.DataModels.Characters;
using Core.ViewModels.Characters.Species;
using Microsoft.AspNetCore.Identity;
using Moq;
using Repositories.Characters;
using Services;
using Services.Characters;
using Services.Google;
using Services.Identity;

namespace Tests.Mocks
{
	public static class ServiceMockFactory
	{
		public static Mock<UserService> UserService(
			int accessFailedCount = 0,
			bool userExists = true,
			bool userCreated = true,
			bool userLockedOut = false,
			bool passwordCorrect = true,
			bool emailConfirmed = true,
			bool verificationCodeMatches = true,
			bool newPasswordValid = true,
			bool hasLoginProviders = true,
			bool deleteSuccessful = true,
			bool addToRoleSuccessful = true,
			bool createUserThrows = false,
			bool removeFromRoleSuccessful = true,
			DateTimeOffset? lockoutEnd = null,
			IdentityResult confirmEmailResult = null,
			string pendingEmail = null)
		{
			confirmEmailResult ??= IdentityResult.Success;

			var describer = new IdentityErrorDescriber();

			var store = IdentityMockFactory.UserStore();
			var options = OptionsMockFactory.IdentityOptions();
			var hasher = IdentityMockFactory.PasswordHasher();
			var userValidators = IdentityMockFactory.UserValidators();
			var passwordValidators = IdentityMockFactory.PasswordValidators();
			var lookupNormalizer = IdentityMockFactory.LookupNormalizer();
			var errorDescriber = IdentityMockFactory.ErrorDescriber();
			var services = AspNetMockFactory.ServiceProvider();
			var logger = LoggerMockFactory.UserManagerLogger();

			var mock = new Mock<UserService>(store.Object, options, hasher.Object, userValidators, passwordValidators, lookupNormalizer.Object, errorDescriber.Object, services.Object, logger.Object);

			// Always-on mocked methods

			mock.Setup(s => s.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
				.ReturnsAsync("your token, sire");

			// Optional mocked methods

			var getUserAsync = mock.Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()));
			var findByIdAsync = mock.Setup(s => s.FindByIdAsync(It.IsAny<string>()));
			var findByNameAsync = mock.Setup(s => s.FindByNameAsync(It.IsAny<string>()));
			var findByEmailAsync = mock.Setup(s => s.FindByEmailAsync(It.IsAny<string>()));
			var updateAsync = mock.Setup(s => s.UpdateAsync(It.IsAny<ApplicationUser>()));

			if (userExists)
			{
				var user = new ApplicationUser
				{
					UserName = "bastion",
					ConcurrencyStamp = "Don't commit if I'm not the same!",
					Email = "no-reply@bastionofshadows.com",
					EmailConfirmed = emailConfirmed,
					Id = "something unique",
					NormalizedUserName = "BASTION",
					NormalizedEmail = "NO-REPLY@bastionofshadows.com",
					AccessFailedCount = accessFailedCount,
					LockoutEnd = lockoutEnd,
					PendingEmail = pendingEmail
				};

				getUserAsync.ReturnsAsync(user);
				findByIdAsync.ReturnsAsync(user);
				findByNameAsync.ReturnsAsync(user);
				findByEmailAsync.ReturnsAsync(user);
				updateAsync.ReturnsAsync(IdentityResult.Success);
			}
			else
			{
				getUserAsync.ReturnsAsync(() => null);
				findByIdAsync.ReturnsAsync(() => null);
				findByNameAsync.ReturnsAsync(() => null);
				findByEmailAsync.ReturnsAsync(() => null);
				updateAsync.ReturnsAsync(IdentityResult.Failed(describer.DefaultError()));
			}

			var createUserResult = userCreated ? IdentityResult.Success : IdentityResult.Failed(describer.DuplicateUserName("bastion"));
			var createAsync = mock.Setup(s => s.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()));

			if (createUserThrows)
			{
				createAsync.Throws<Exception>();
			}
			else
			{
				createAsync.ReturnsAsync(createUserResult)
						   .Verifiable();
			}

			mock.Setup(s => s.IsLockedOutAsync(It.IsAny<ApplicationUser>()))
				.ReturnsAsync(userExists && userLockedOut);

			mock.Setup(s => s.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
				.ReturnsAsync(passwordCorrect);

			mock.Setup(s => s.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
				.ReturnsAsync(confirmEmailResult)
				.Verifiable();

			mock.Setup(s => s.GenerateChangeEmailTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
				.ReturnsAsync("your token, sire");

			mock.Setup(s => s.ChangeEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(verificationCodeMatches ? IdentityResult.Success : IdentityResult.Failed(describer.InvalidToken()));

			mock.Setup(s => s.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>()))
				.ReturnsAsync("abc123");

			mock.Setup(s => s.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(newPasswordValid ? IdentityResult.Success : IdentityResult.Failed(describer.PasswordRequiresDigit()));

			mock.Setup(s => s.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
				.ReturnsAsync(newPasswordValid ? IdentityResult.Success : IdentityResult.Failed(describer.PasswordRequiresDigit()));

			mock.Setup(s => s.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(newPasswordValid ? IdentityResult.Success : IdentityResult.Failed(describer.PasswordRequiresDigit()));

			mock.Setup(s => s.GetLoginsAsync(It.IsAny<ApplicationUser>()))
				.ReturnsAsync(hasLoginProviders ? new List<UserLoginInfo> {new UserLoginInfo("Bastion", "abc123", "Bastion of Shadows")} : new List<UserLoginInfo>());

			mock.Setup(s => s.DeleteAsync(It.IsAny<ApplicationUser>()))
				.ReturnsAsync(deleteSuccessful ? IdentityResult.Success : IdentityResult.Failed(describer.ConcurrencyFailure()));

			mock.Setup(s => s.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
				.ReturnsAsync(addToRoleSuccessful ? IdentityResult.Success : IdentityResult.Failed(describer.UserAlreadyInRole("bastion")))
				.Verifiable();

			mock.Setup(s => s.AddToRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
				.ReturnsAsync(addToRoleSuccessful ? IdentityResult.Success : IdentityResult.Failed(describer.UserAlreadyInRole(Roles.Admin)));

			mock.Setup(s => s.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
				.ReturnsAsync(removeFromRoleSuccessful ? IdentityResult.Success : IdentityResult.Failed(describer.UserNotInRole(Roles.Admin)));

			return mock;
		}

		public static Mock<SignInService> SignInService(bool passwordCorrect = true, SignInResult signInResult = null)
		{
			signInResult ??= passwordCorrect ? SignInResult.Success : SignInResult.Failed;

			var userService = UserService()
				.Object;
			var accessor = IdentityMockFactory.ContextAccessor()
											  .Object;
			var claimsFactory = IdentityMockFactory.ClaimsFactory()
												   .Object;
			var options = OptionsMockFactory.IdentityOptions();
			var logger = LoggerMockFactory.SignInManagerLogger()
										  .Object;
			var schemes = IdentityMockFactory.AuthenticationSchemeProvider()
											 .Object;
			var confirmation = IdentityMockFactory.UserConfirmation()
												  .Object;

			var mock = new Mock<SignInService>(userService, accessor, claimsFactory, options, logger, schemes, confirmation);

			mock.Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), true))
				.ReturnsAsync(signInResult);

			mock.Setup(s => s.SignOutAsync())
				.Verifiable();

			return mock;
		}

		public static Mock<RoleManager<Role>> RoleManager(bool roleCreated = true, bool roleExists = true)
		{
			var roleStore = IdentityMockFactory.RoleStore();
			var roleValidator = IdentityMockFactory.RoleValidator();
			var keyNormalizer = IdentityMockFactory.LookupNormalizer();
			var describer = new IdentityErrorDescriber();
			var logger = LoggerMockFactory.RoleManagerLogger();

			var roleValidators = new List<IRoleValidator<Role>> {roleValidator.Object};

			var manager = new Mock<RoleManager<Role>>(
				roleStore.Object,
				roleValidators,
				keyNormalizer.Object,
				describer,
				logger.Object);

			manager.Setup(m => m.CreateAsync(It.IsAny<Role>()))
				   .ReturnsAsync(roleCreated ? IdentityResult.Success : IdentityResult.Failed(describer.InvalidRoleName("bastion")))
				   .Verifiable();

			manager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
				   .ReturnsAsync(roleExists ? new Role(Roles.Admin) : null);
			
			var db = SqliteInMemoryDatabaseFactory.GetNewDb(false);
			if (roleExists)
			{
				db.Roles.Add(new Role(Roles.Admin));
				db.SaveChanges();
			}
			manager.SetupGet(m => m.Roles)
				   .Returns(db.Roles);

			return manager;
		}

		public static Mock<IEmailSenderService> EmailSenderService(string subject = "")
		{
			return new Mock<IEmailSenderService>();
		}

		public static Mock<RecaptchaService> RecaptchaService()
		{
			var client = NetworkMockFactory.GeneralHttpClient()
										   .Object;
			var logger = LoggerMockFactory.DefaultLogger()
										  .Object;
			var config = ConfigurationMockFactory.RecaptchaSecretKeyConfiguration()
												 .Object;
			return new Mock<RecaptchaService>(client, logger, config);
		}

		public static RecaptchaService FailingRecaptchaService()
		{
			var handler = NetworkMockFactory.ThrowsHandler(new HttpRequestException())
											.Object;
			var client = new HttpClient(handler);
			var logger = LoggerMockFactory.DefaultLogger()
										  .Object;
			var config = ConfigurationMockFactory.RecaptchaSecretKeyConfiguration()
												 .Object;
			return new RecaptchaService(client, logger, config);
		}

		public static RecaptchaService SucceedingRecaptchaService()
		{
			var handler = NetworkMockFactory.GeneralHandler(message: new StringContent("{\"success\": true}"))
											.Object;
			var client = new HttpClient(handler);
			var logger = LoggerMockFactory.DefaultLogger()
										  .Object;
			var config = ConfigurationMockFactory.RecaptchaSecretKeyConfiguration()
												 .Object;
			return new RecaptchaService(client, logger, config);
		}

		public static Mock<ISpeciesService> SpeciesService(bool successful = true)
		{
			var mock = new Mock<ISpeciesService>();

			var getSpecies = mock.Setup(s => s.GetSpecies(It.IsAny<int>()));
			var addSpecies = mock.Setup(s => s.AddSpecies(It.IsAny<AddSpeciesModel>()));
			var editSpecies = mock.Setup(s => s.EditSpecies(It.IsAny<EditSpeciesModel>()));

			if (successful)
			{
				getSpecies.ReturnsAsync(new Species
				{
					Id = 1,
					Name = "Human",
					PluralName = "Humans",
					Description = "Humans are good and cool",
					ForceSensitive = true,
					HpCoefficient = 10.0f
				});
				addSpecies.ReturnsAsync(ServiceResult.Success);
				editSpecies.ReturnsAsync(ServiceResult.Success);
			}
			else
			{
				addSpecies.ReturnsAsync(new ServiceResult("Mock this bad service result"));
				editSpecies.ReturnsAsync(new ServiceResult("Mock this bad service result"));
			}

			mock.Setup(s => s.GetSpecies())
				.ReturnsAsync(new List<Species>
				{
					new Species
					{
						Id = 1,
						Name = "Human",
						PluralName = "Humans",
						Description = "Humans are good and cool",
						HpCoefficient = 10.0f,
						ForceSensitive = true,
						AwarenessModifier = 0,
						CharismaModifier = 0,
						ConstitutionModifier = 0,
						DexterityModifier = 0,
						IntelligenceModifier = 0,
						StrengthModifier = 0,
						WisdomModifier = 0
					}
				});

			return mock;
		}
	}
}