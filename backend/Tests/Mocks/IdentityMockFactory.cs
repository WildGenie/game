using System.Collections.Generic;
using Core.DataModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.Mocks
{
	public static class IdentityMockFactory
	{
		public static Mock<IPasswordValidator<ApplicationUser>> PasswordValidator()
		{
			var mock = new Mock<IPasswordValidator<ApplicationUser>>();
			mock.Setup(p => p.ValidateAsync(It.IsAny<UserManager<ApplicationUser>>(), It.IsAny<ApplicationUser>(), It.IsAny<string>()))
							 .ReturnsAsync(IdentityResult.Success);
			return mock;
		}

		public static IList<IPasswordValidator<ApplicationUser>> PasswordValidators(bool includeMockedValidator = false)
		{
			var mock = new List<IPasswordValidator<ApplicationUser>>();
			if (includeMockedValidator)
				mock.Add(PasswordValidator().Object);
			
			return mock;
		}

		public static IList<IUserValidator<ApplicationUser>> UserValidators()
		{
			return new List<IUserValidator<ApplicationUser>>();
		}
		
		public static Mock<IPasswordHasher<ApplicationUser>> PasswordHasher()
		{
			var mock = new Mock<IPasswordHasher<ApplicationUser>>();
			mock.Setup(p => p.HashPassword(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
				.Returns("hashed password");
			return mock;
		}

		public static Mock<IHttpContextAccessor> ContextAccessor()
		{
			return new Mock<IHttpContextAccessor>();
		}

		public static Mock<IUserClaimsPrincipalFactory<ApplicationUser>> ClaimsFactory()
		{
			return new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
		}

		public static Mock<IAuthenticationSchemeProvider> AuthenticationSchemeProvider()
		{
			return new Mock<IAuthenticationSchemeProvider>();
		}

		public static Mock<IUserConfirmation<ApplicationUser>> UserConfirmation()
		{
			return new Mock<IUserConfirmation<ApplicationUser>>();
		}

		public static Mock<IUserStore<ApplicationUser>> UserStore()
		{
			var mock = new Mock<IUserStore<ApplicationUser>>();
			mock.As<IUserPasswordStore<ApplicationUser>>();
			return mock;
		}

		public static Mock<IRoleStore<Role>> RoleStore()
		{
			return new Mock<IRoleStore<Role>>();
		}

		public static Mock<IRoleValidator<Role>> RoleValidator()
		{
			return new Mock<IRoleValidator<Role>>();
		}
		
		public static Mock<ILookupNormalizer> LookupNormalizer()
		{
			return new Mock<ILookupNormalizer>();
		}

		public static Mock<IdentityErrorDescriber> ErrorDescriber()
		{
			return new Mock<IdentityErrorDescriber>();
		}
	}
}