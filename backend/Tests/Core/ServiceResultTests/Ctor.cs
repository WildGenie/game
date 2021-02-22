using Core;
using Xunit;

namespace Tests.Core.ServiceResultTests
{
	public class Ctor
	{
		[Fact]
		public void SuccessReturnsSuccessfulServiceResult()
		{
			var result = ServiceResult.Success;

			Assert.True(result.WasSuccessful);
			Assert.Null(result.Message);
		}

		[Fact]
		public void ServiceResultWithMessageArgReturnsFailingServiceResult()
		{
			var result = new ServiceResult("There was a problem");

			Assert.False(result.WasSuccessful);
			Assert.Equal("There was a problem", result.Message);
		}
	}
}