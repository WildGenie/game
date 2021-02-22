using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Core.DataModels
{
	[ExcludeFromCodeCoverage]
	public class Role : IdentityRole
	{
		public virtual ICollection<UserRole> UserRoles { get; set; }
		public Role() : base() { }

		public Role(string name) : base(name) { }
	}
}