using Backend.Tools;
using Xunit;

namespace Tests.Backend.Tools.ApiResponseTests
{
	public class CtorTResult
	{
		[Fact]
		public void SetsUpCorrectResult()
		{
			var response = new ApiResponse<int>(5);
			
			Assert.Equal(5, response.Result);
		}

		[Fact]
		public void ErrorsIsNullIfNotSet()
		{
			var response = new ApiResponse<string>();
			
			Assert.Null(response.Errors);
		}

		[Fact]
		public void ResultIsDefaultIfNotSet()
		{
			var response = new ApiResponse<int>();
			
			Assert.Equal(0, response.Result);
		}
	}
}