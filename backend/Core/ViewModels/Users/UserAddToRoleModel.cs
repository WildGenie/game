using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.ViewModels.Users
{
	[ExcludeFromCodeCoverage]
	public class UserAddToRoleModel
	{
		[Required]
		public string UserId { get; set; }

		[Required]
		public List<string> Roles { get; set; }
	}
}