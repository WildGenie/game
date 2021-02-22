using System.Diagnostics.CodeAnalysis;

namespace Core
{
	[ExcludeFromCodeCoverage]
	public class ServiceResult
	{
		public string Message { get; set; }
		public bool WasSuccessful { get; set; }
	}
}