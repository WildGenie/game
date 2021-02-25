using System.Diagnostics.CodeAnalysis;

namespace Core
{
	public class ServiceResult
	{
		[ExcludeFromCodeCoverage]
		public string Message { get; set; }
		[ExcludeFromCodeCoverage]
		public bool WasSuccessful { get; set; }

		public static ServiceResult Success => new ServiceResult {WasSuccessful = true};

		[ExcludeFromCodeCoverage]
		public ServiceResult() { }

		public ServiceResult(string message)
		{
			Message = message;
			WasSuccessful = false;
		}
	}

	public class ServiceResult<TResult> : ServiceResult
	{
		public TResult Result { get; }
		
		public ServiceResult(string message) : base(message) { }
		
		public ServiceResult(TResult result)
		{
			WasSuccessful = true;
			Result = result;
		}
	}
}