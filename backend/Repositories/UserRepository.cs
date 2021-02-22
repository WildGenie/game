using System.Diagnostics.CodeAnalysis;
using Core.DataModels;

namespace Repositories
{
	[ExcludeFromCodeCoverage]
	public class UserRepository : Repository<ApplicationUser, string>, IUserRepository
	{
		public UserRepository(ApplicationDbContext context) : base(context) { }
	}
}