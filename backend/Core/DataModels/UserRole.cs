using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Core.DataModels
{
	[ExcludeFromCodeCoverage]
	public class UserRole : IdentityUserRole<string>
	{
		public virtual ApplicationUser User { get; set; }
		public virtual Role Role { get; set; }
	}
}