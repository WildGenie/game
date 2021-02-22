using Backend.Tools;
using Core;
using Xunit;

namespace Tests.Backend.Tools.ApiResponseTests
{
	public class CtorServiceResult
	{
		private const string Message = "There was a problem";
		
		[Fact]
		public void AddsGeneralKeyToErrorDictionary()
		{
			var response = new ApiResponse(new ServiceResult(Message));
			
			Assert.NotNull(response.Errors);
			Assert.NotEmpty(response.Errors);
			Assert.Contains("General", response.Errors.Keys);
		}

		[Fact]
		public void AddsMessageToGeneralErrorList()
		{
			var response = new ApiResponse(new ServiceResult(Message));
			
			Assert.Contains(Message, response.Errors["General"]);
		}
	}
}