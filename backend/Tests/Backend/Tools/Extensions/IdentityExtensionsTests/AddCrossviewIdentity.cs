using System;
using System.Linq;
using System.Threading.Tasks;
using Backend;
using Backend.Tools.Extensions;
using Core.DataModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Repositories;
using Services.Identity;
using Xunit;

namespace Tests.Backend.Tools.Extensions.IdentityExtensionsTests
{
	public class AddCrossviewIdentity
	{
		[Fact]
		public void CallsAddCustomDatabase()
		{
			var config = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json")
						 .Build();
			var services = new ServiceCollection();
			services.AddCrossviewIdentity(config);
			var container = services.BuildServiceProvider();

			var db = container.GetService<ApplicationDbContext>();
			Assert.NotNull(db);
		}

		[Fact]
		public void CallsAddCustomIdentity()
		{
			var config = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json")
						 .Build();
			var services = new ServiceCollection();
			services.AddCrossviewIdentity(config);
			var container = services.BuildServiceProvider();

			// AddIdentityCore
			var userValidator = container.GetService<IUserValidator<ApplicationUser>>();
			Assert.NotNull(userValidator);

			// AddSignInManager
			var signInService = container.GetService<SignInService>();
			Assert.NotNull(signInService);

			// AddUserManager
			var userService = container.GetService<UserService>();
			Assert.NotNull(userService);

			// AddRoles
			var roleManager = container.GetService<RoleManager<Role>>();
			Assert.NotNull(roleManager);

			// AddRoleStore
			var roleStore = container.GetService<IRoleStore<Role>>();
			Assert.NotNull(roleStore);

			// AddEntityFrameworkStores
			var userStore = container.GetService<IUserStore<ApplicationUser>>();
			Assert.NotNull(userStore);

			// AddDefaultTokenProviders
			var dataProtectorTokenProvider = container.GetService<DataProtectorTokenProvider<ApplicationUser>>();
			Assert.NotNull(dataProtectorTokenProvider);

			// Add ISecurityStampValidator
			var securityStampValidator = container.GetService<ISecurityStampValidator>();
			Assert.NotNull(securityStampValidator);
		}

		[Fact]
		public void CallsAddCustomAuthentication()
		{
			var config = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json")
						 .Build();
			var services = new ServiceCollection();
			services.AddCrossviewIdentity(config);
			var container = services.BuildServiceProvider();

			// AddAuthentication
			var auth = container.GetService<IAuthenticationService>();
			Assert.NotNull(auth);

			// AddIdentityCookies
			var cookies = container.GetService<IOptions<CookieAuthenticationOptions>>();
			Assert.NotNull(cookies);
		}

		[Fact]
		public void CallsAddCustomAuthorization()
		{
			var config = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json")
						 .Build();
			var services = new ServiceCollection();
			services.AddCrossviewIdentity(config);
			var container = services.BuildServiceProvider();

			var authorizationOptions = container.GetService<IOptions<AuthorizationOptions>>();
			Assert.NotNull(authorizationOptions);

			var instance = authorizationOptions.Value.FallbackPolicy.Requirements.FirstOrDefault(r => r is DenyAnonymousAuthorizationRequirement);
			Assert.NotNull(instance);
		}

		[Fact]
		public void CallsAddCustomIdentityCookie()
		{
			var config = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json")
						 .Build();
			var services = new ServiceCollection();
			services.AddCrossviewIdentity(config);
			var container = services.BuildServiceProvider();

			// var cookies = container.GetService<IOptions<CookieAuthenticationOptions>>()
			var options = container.GetService<IOptionsSnapshot<CookieAuthenticationOptions>>()
								   .Get("Identity.Application");

			Assert.NotNull(options);
			Assert.Equal("auth_cookie", options.Cookie.Name);
			Assert.Equal(SameSiteMode.Strict, options.Cookie.SameSite);
			Assert.Equal(TimeSpan.FromHours(2), options.ExpireTimeSpan);
			// Assert.Equal(PathString.Empty, options.LoginPath);
			// Assert.Equal(PathString.Empty, options.LogoutPath);
			// Assert.Equal(PathString.Empty, options.AccessDeniedPath);
			Assert.True(options.SlidingExpiration);

			var httpContext = new DefaultHttpContext();

			var authScheme = new AuthenticationScheme("stub", "Crossview Stub", typeof(IAuthenticationHandler));
			var authProps = new AuthenticationProperties();

			var unauthorizedContext = new RedirectContext<CookieAuthenticationOptions>(httpContext, authScheme, options, authProps, string.Empty);
			var unauthorizedResult = options.Events.OnRedirectToLogin(unauthorizedContext);

			Assert.NotNull(unauthorizedResult);
			Assert.IsType<Task>(unauthorizedResult);
			Assert.Equal(StatusCodes.Status401Unauthorized, unauthorizedContext.Response.StatusCode);
				
			var logoutContext = new RedirectContext<CookieAuthenticationOptions>(httpContext, authScheme, options, authProps, string.Empty);
			var logoutResult = options.Events.OnRedirectToLogout(logoutContext);

			Assert.NotNull(logoutResult);
			Assert.IsType<Task>(logoutResult);
			Assert.Equal(StatusCodes.Status204NoContent, logoutContext.Response.StatusCode);

			var forbiddenContext = new RedirectContext<CookieAuthenticationOptions>(httpContext, authScheme, options, authProps, string.Empty);
			var forbiddenResult = options.Events.OnRedirectToAccessDenied(forbiddenContext);
			
			Assert.NotNull(forbiddenResult);
			Assert.IsType<Task>(forbiddenResult);
			Assert.Equal(StatusCodes.Status403Forbidden, forbiddenContext.Response.StatusCode);
		}

		[Fact]
		public void CallsConfigureCustomIdentity()
		{
			var config = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json")
						 .Build();
			var services = new ServiceCollection();
			services.AddCrossviewIdentity(config);
			var container = services.BuildServiceProvider();

			var options = container.GetService<IOptions<IdentityOptions>>()?.Value;
			Assert.NotNull(options);
			
			// Password settings
			Assert.True(options.Password.RequireDigit);
			Assert.True(options.Password.RequireLowercase);
			Assert.True(options.Password.RequireNonAlphanumeric);
			Assert.True(options.Password.RequireUppercase);
			Assert.Equal(8, options.Password.RequiredLength);
			Assert.Equal(1, options.Password.RequiredUniqueChars);
			
			// Lockout settings
			Assert.Equal(TimeSpan.FromMinutes(15), options.Lockout.DefaultLockoutTimeSpan);
			Assert.Equal(5, options.Lockout.MaxFailedAccessAttempts);
			Assert.True(options.Lockout.AllowedForNewUsers);
			
			// User settings
			Assert.DoesNotMatch(@"\W", options.User.AllowedUserNameCharacters);
			Assert.True(options.User.RequireUniqueEmail);
			Assert.True(options.SignIn.RequireConfirmedEmail);
		}
	}
}