using System.Collections.Generic;
using Core.DataModels;
using Core.ViewModels.Account;
using Xunit;

namespace Tests.Core.ViewModels.Account.ApplicationUserModelTests
{
	public class Roles
	{
		[Fact]
		public void HasEmptyRolesByDefault()
		{
			var model = new ApplicationUserModel();
			
			Assert.Empty(model.Roles);
		}

		[Fact]
		public void HasEmptyRolesIfNoRolesPassed()
		{
			var user = new ApplicationUser();
			
			var model = new ApplicationUserModel(user);
			
			Assert.Empty(model.Roles);
		}

		[Fact]
		public void RolesContainsRoleNamesPassed()
		{
			var user = new ApplicationUser
			{
				UserRoles = new List<UserRole>
				{
					new UserRole
					{
						Role = new Role("user")
					},
					new UserRole
					{
						Role = new Role("admin")
					}
				}
			};
			
			var model = new ApplicationUserModel(user);
			
			Assert.NotEmpty(model.Roles);
			Assert.Equal(2, model.Roles.Count);
			Assert.Equal("user", model.Roles[0]);
		}
	}
}