using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Core.DataModels
{
	public class ApplicationUser : IdentityUser
	{
		[PersonalData]
		[ExcludeFromCodeCoverage]
		public string PendingEmail { get; set; }

		[ExcludeFromCodeCoverage]
		public virtual ICollection<UserRole> UserRoles { get; set; }

		[NotMapped]
		public virtual IEnumerable<Role> Roles => UserRoles?.Select(ur => ur.Role)
														   .ToList() ?? new List<Role>();
	}
}