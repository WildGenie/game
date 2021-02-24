using System.Diagnostics.CodeAnalysis;

namespace Core.Options
{
	[ExcludeFromCodeCoverage]
	public class EmailOptions
	{
		public string SendGridApiKey { get; set; }
		public string EmailFromAddress { get; set; }
		public string EmailFromName { get; set; }
		public string ApplicationUrl { get; set; }
	}
}