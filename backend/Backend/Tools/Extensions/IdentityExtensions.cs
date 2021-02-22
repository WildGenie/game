using System;
using System.Threading.Tasks;
using Core.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Repositories;
using Services.Identity;

namespace Backend.Tools.Extensions
{
	public static class IdentityExtensions
	{
		public static IServiceCollection AddCrossviewIdentity(this IServiceCollection services, IConfiguration config)
		{
			return services.AddCustomDatabase(config)
						   .AddCustomIdentity()
						   .AddCustomAuthentication()
						   .AddCustomIdentityCookie()
						   .AddCustomAuthorization()
						   .ConfigureCustomIdentity();
		}

		public static IServiceCollection AddCustomDatabase(this IServiceCollection services, IConfiguration config)
		{
			// Set up the database with lazy loading
			services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseMySql(config.GetConnectionString("Default"))
					   .UseLazyLoadingProxies();
			});

			return services;
		}

		public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
		{
			// Set up Identity Core and Identity's EF Core
			services.AddIdentityCore<ApplicationUser>(o => { o.Stores.MaxLengthForKeys = 128; })
					.AddSignInManager<SignInService>()
					.AddUserManager<UserService>()
					.AddRoles<Role>()
					.AddRoleStore<RoleStore<Role, ApplicationDbContext, string, UserRole, RoleClaim>>()
					.AddEntityFrameworkStores<ApplicationDbContext>()
					.AddDefaultTokenProviders();

			services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<ApplicationUser>>();

			return services;
		}

		public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
		{
			// Set up Authentication
			services.AddAuthentication(o =>
					{
						o.DefaultScheme = IdentityConstants.ApplicationScheme;
						o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
					})
					.AddIdentityCookies(o => { });

			return services;
		}

		public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
		{
			// Configure Authorization pipelines
			services.AddAuthorization(options =>
			{
				options.FallbackPolicy = new AuthorizationPolicyBuilder()
										 .RequireAuthenticatedUser()
										 .Build();
			});

			return services;
		}

		public static IServiceCollection AddCustomIdentityCookie(this IServiceCollection services)
		{
			// Set up Identity Cookie
			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.Name = "auth_cookie";
				options.Cookie.SameSite = SameSiteMode.Strict;
				options.ExpireTimeSpan = TimeSpan.FromHours(2);
				options.LoginPath = PathString.Empty;
				options.LogoutPath = PathString.Empty;
				options.AccessDeniedPath = PathString.Empty;
				options.SlidingExpiration = true;

				// This allows the API to send 401 responses instead of redirecting to a login page
				options.Events.OnRedirectToLogin = context =>
				{
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;
					return Task.CompletedTask;
				};

				// This allows the API to send 403 responses instead of redirecting to a login page
				options.Events.OnRedirectToAccessDenied = context =>
				{
					context.Response.StatusCode = StatusCodes.Status403Forbidden;
					return Task.CompletedTask;
				};

				// This allows the API to send 204 responses instead of redirecting to a logout page
				options.Events.OnRedirectToLogout = context =>
				{
					context.Response.StatusCode = StatusCodes.Status204NoContent;
					return Task.CompletedTask;
				};
			});

			return services;
		}

		public static IServiceCollection ConfigureCustomIdentity(this IServiceCollection services)
		{
			// Configure Identity Password, Username, Email, Account settings
			services.Configure<IdentityOptions>(options =>
			{
				// Password settings.
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 8;
				options.Password.RequiredUniqueChars = 1;

				// Lockout settings.
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.AllowedForNewUsers = true;

				// User settings.
				options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
				options.User.RequireUniqueEmail = true;

				options.SignIn.RequireConfirmedEmail = true;
			});

			return services;
		}
	}
}