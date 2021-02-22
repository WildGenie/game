using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Services.Identity
{
	public class UserService : UserManager<ApplicationUser>
	{
		public UserService(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) { }

		public virtual async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string newPassword)
		{
			var result = await UpdatePasswordHash(user, newPassword, true);
			if (result.Succeeded)
			{
				await UpdateAsync(user);
			}

			return result;
		}
	}
}