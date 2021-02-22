using Core.DataModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Services.Identity
{
	public class SignInService : SignInManager<ApplicationUser>
	{
		public SignInService(UserService userService,
			IHttpContextAccessor contextAccessor,
			IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
			IOptions<IdentityOptions> optionsAccessor,
			ILogger<SignInManager<ApplicationUser>> logger,
			IAuthenticationSchemeProvider schemes,
			IUserConfirmation<ApplicationUser> confirmation)
			: base(userService, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation) { }
	}
}