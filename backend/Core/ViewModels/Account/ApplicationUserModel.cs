using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Core.DataModels;

namespace Core.ViewModels.Account
{
	public class ApplicationUserModel
	{
		[ExcludeFromCodeCoverage]
		public string Id { get; set; }
		
		[ExcludeFromCodeCoverage]
		public string UserName { get; set; }
		
		[ExcludeFromCodeCoverage]
		public string Email { get; set; }
		
		[ExcludeFromCodeCoverage]
		public bool IsVerified { get; set; }
		
		public IList<string> Roles { get; set; }

		public ApplicationUserModel()
		{
			Roles = new List<string>();
		}

		public ApplicationUserModel(ApplicationUser user) : this()
		{
			Id = user.Id;
			UserName = user.UserName;
			Email = user.Email;
			IsVerified = user.EmailConfirmed;
			Roles = user.Roles.Select(r => r.Name)
						.ToList();
		}
	}
}