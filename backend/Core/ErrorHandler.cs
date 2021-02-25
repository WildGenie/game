using System;

namespace Core
{
	public static class ErrorHandler
	{
		const string DatabaseMessage = "A database error occurred. Please try again later.";
		public static ServiceResult HandleDbError(Exception e)
		{
			return new ServiceResult(DatabaseMessage);
		}

		public static ServiceResult<T> HandleDbError<T>(Exception e)
		{
			return new ServiceResult<T>(DatabaseMessage);
		}
	}
}