using System;
using Core;
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
	}
}