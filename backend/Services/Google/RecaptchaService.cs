using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core;
using Core.DataModels.Google;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Serilog;

namespace Services.Google
{
	public class RecaptchaService
	{
		private readonly HttpClient _client;
		private readonly ILogger _logger;
		private readonly string _secret;

		public RecaptchaService(HttpClient client, ILogger logger, IConfiguration configuration)
		{
			_client = client;
			_logger = logger;
			_secret = configuration["Google:Recaptcha:secretKey"];

			_logger.Information("Created RecaptchaService instance");
		}

		public async Task<ServiceResult> VerifyRecaptcha(string token)
		{
			var pars = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("secret", _secret),
				new KeyValuePair<string, string>("response", token)
			};
			var uri = new Uri("https://www.google.com/recaptcha/api/siteverify");
			var content = new FormUrlEncodedContent(pars);

			HttpResponseMessage message;
			try
			{
				_logger.Information("Sending Google reCAPTCHA verification");
				message = await _client.PostAsync(uri, content);
			}
			catch (HttpRequestException e)
			{
				_logger.Error("Unable to contact reCAPTCHA service: {@exception}", e);

				return new ServiceResult
				{
					WasSuccessful = false,
					Message = ErrorMessages.RecaptchaNoConnection
				};
			}

			var responseMessage = await message.Content.ReadAsStringAsync();
			var response = JsonSerializer.Deserialize<RecaptchaResponse>(responseMessage);
			var result = new ServiceResult
			{
				WasSuccessful = response.Success
			};

			var errorMessage = GetRecaptchaErrorMessage(response.ErrorCodes);
			if (!string.IsNullOrEmpty(errorMessage))
			{
				result.Message = errorMessage;
			}

			return result;
		}

		private string GetRecaptchaErrorMessage(IList<string> errors)
		{
			if (errors == null || errors.Count == 0)
			{
				return string.Empty;
			}

			if (errors.Contains("missing-input-secret") ||
				errors.Contains("invalid-input-secret") ||
				errors.Contains("bad-request"))
			{
				_logger.Error("reCAPTCHA reported application-level errors: {@errors}", errors);
				return "Application error";
			}

			if (errors.Contains("timeout-or-duplicate"))
			{
				_logger.Error("reCAPTCHA reported a duplicate or timed-out response");
				return "reCAPTCHA token expired";
			}

			if (errors.Contains("missing-input-response"))
			{
				_logger.Error("reCAPTCHA reported a missing user token");
				return "reCAPTCHA token missing";
			}

			if (errors.Contains("invalid-input-response"))
			{
				_logger.Error("reCAPTCHA reported an invalid user token");
				return "reCAPTCHA token invalid";
			}

			_logger.Error("reCAPTCHA contains errors not currently accounted for by the application: {@errors}", errors);
			return "Unknown reCAPTCHA error";
		}
	}
}