using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Core.DataModels.Google
{
	[ExcludeFromCodeCoverage]
	public class RecaptchaResponse
	{
		[JsonPropertyName("success")]
		public bool Success { get; set; }

		[JsonPropertyName("challenge_ts")]
		public DateTime ChallengeTs { get; set; }

		public string Hostname { get; set; }

		[JsonPropertyName("error-codes")]
		public List<string> ErrorCodes { get; set; }
	}
}