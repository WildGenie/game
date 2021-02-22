using System.Collections.Generic;
using Backend.Tools;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Tests.Backend.Tools.ApiResponseTests
{
	public class CtorIdentityErrors
	{
		private IdentityErrorDescriber _describer = new IdentityErrorDescriber();
		
		[Fact]
		public void AddsEmailKeyIfErrorCodeMatches()
		{
			var errors = new List<IdentityError>
			{
				_describer.DuplicateEmail("no-reply@crossviewsoftware.io")
			};

			var response = new ApiResponse(errors);

			Assert.Contains("Email", response.Errors.Keys);
			Assert.Single(response.Errors["Email"]);
			Assert.Equal("Email 'no-reply@crossviewsoftware.io' is already taken.", response.Errors["Email"][0]);
		}

		[Fact]
		public void AddsUserNameKeyIfErrorCodeMatches()
		{
			var errors = new List<IdentityError>
			{
				_describer.InvalidUserName("crossview")
			};
			
			var response = new ApiResponse(errors);

			Assert.Contains("UserName", response.Errors.Keys);
			Assert.Single(response.Errors["UserName"]);
			Assert.Equal("User name 'crossview' is invalid, can only contain letters or digits.", response.Errors["UserName"][0]);
		}
		
		[Fact]
		public void AddsPasswordKeyIfErrorCodeMatches()
		{
			var errors = new List<IdentityError>
			{
				_describer.PasswordMismatch()
			};
			
			var response = new ApiResponse(errors);

			Assert.Contains("Password", response.Errors.Keys);
			Assert.Single(response.Errors["Password"]);
			Assert.Equal("Incorrect password.", response.Errors["Password"][0]);
		}
		
		[Fact]
		public void AddsRoleNameKeyIfErrorCodeMatches()
		{
			var errors = new List<IdentityError>
			{
				_describer.InvalidRoleName("crossview")
			};
			
			var response = new ApiResponse(errors);

			Assert.Contains("RoleName", response.Errors.Keys);
			Assert.Single(response.Errors["RoleName"]);
			Assert.Equal("Role name 'crossview' is invalid.", response.Errors["RoleName"][0]);
		}
		
		[Fact]
		public void AddsGeneralKeyIfErrorCodeMatches()
		{
			var errors = new List<IdentityError>
			{
				_describer.DefaultError()
			};
			
			var response = new ApiResponse(errors);

			Assert.Contains("General", response.Errors.Keys);
			Assert.Single(response.Errors["General"]);
			Assert.Equal("An unknown failure has occurred.", response.Errors["General"][0]);
		}
	}
}