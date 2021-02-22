using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Core.DataModels
{
	[ExcludeFromCodeCoverage]
	public class RoleClaim : IdentityRoleClaim<string> { }
}