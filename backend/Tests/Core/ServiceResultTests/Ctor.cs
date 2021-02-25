using Core;
using Core.DataModels.Characters;
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

		[Fact]
		public void ReserviceResultWithResultArgReturnsSuccessfulServiceResult()
		{
			var species = new Species
			{
				Name = "Human"
			};
			var result = new ServiceResult<Species>(species);

			Assert.True(result.WasSuccessful);
			Assert.Equal("Human", result.Result.Name);
		}

		[Fact]
		public void ReturnsServiceResultOfTWithErrorMessage()
		{
			var result = new ServiceResult<Species>("An error occurred");

			Assert.False(result.WasSuccessful);
			Assert.Equal("An error occurred", result.Message);
		}
	}
}