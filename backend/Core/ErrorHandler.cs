using System;

namespace Core
{
	public static class ErrorHandler
	{
		public static ServiceResult HandleDbError(Exception e)
		{
			return new ServiceResult("A database error occurred. Please try again later.");
		}
	}
}