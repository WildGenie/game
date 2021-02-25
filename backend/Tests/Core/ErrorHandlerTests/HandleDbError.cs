using System;
using Core;
using Core.DataModels.Characters;
using Xunit;

namespace Tests.Core.ErrorHandlerTests
{
	public class HandleDbError
	{
		[Fact]
		public void ReturnsServiceResultWithMessage()
		{
			var e = new Exception();
			var result = ErrorHandler.HandleDbError(e);

			Assert.IsType<ServiceResult>(result);
			Assert.Equal("A database error occurred. Please try again later.", result.Message);
		}

		[Fact]
		public void ReturnsServiceResultTWithMessage()
		{
			var e = new Exception();
			var result = ErrorHandler.HandleDbError<Species>(e);

			Assert.IsType<ServiceResult<Species>>(result);
			Assert.Equal("A database error occurred. Please try again later.", result.Message);
		}
	}
}