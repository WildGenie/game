using Backend.Tools;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace Tests.Backend.Tools.ApiResponseTests
{
	public class CtorModelStateDictionary
	{
		[Fact]
		public void AddsEmptyDictionary()
		{
			var response = new ApiResponse(new ModelStateDictionary());
			
			Assert.NotNull(response.Errors);
			Assert.Empty(response.Errors);
		}

		[Fact]
		public void AddsErrorsFromModelStateDictionary()
		{
			var errors = new ModelStateDictionary();
			var key1 = "movies";
			var key2 = "albums";
			var movie1 = "Avatar";
			var movie2 = "Fellowship of the Ring";
			var movie3 = "The Empire Strikes Back";
			var album1 = "Zeppelin IV";
			var album2 = "Houses of the Holy";
			var album3 = "It's Pronounced \"LEH-nerd SKIN-nerd\"";
			
			errors.AddModelError(key1, movie1);
			errors.AddModelError(key1, movie2);
			errors.AddModelError(key1, movie3);
			
			errors.AddModelError(key2, album1);
			errors.AddModelError(key2, album2);
			errors.AddModelError(key2, album3);

			var response = new ApiResponse(errors);
			
			Assert.Equal(2, response.Errors.Keys.Count);

			Assert.Contains(key1, response.Errors.Keys);
			Assert.Contains(key2, response.Errors.Keys);
			
			Assert.Equal(3, response.Errors[key1].Count);
			Assert.Contains(movie1, response.Errors[key1]);
			Assert.Contains(movie2, response.Errors[key1]);
			Assert.Contains(movie3, response.Errors[key1]);
			
			Assert.Equal(3, response.Errors[key2].Count);
			Assert.Contains(album1, response.Errors[key2]);
			Assert.Contains(album2, response.Errors[key2]);
			Assert.Contains(album3, response.Errors[key2]);
		}
	}
}