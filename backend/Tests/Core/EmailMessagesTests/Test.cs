using System;
using Core;
using Core.Options;
using Microsoft.Extensions.Options;
using Tests.Mocks;
using Xunit;

namespace Tests.Core.EmailMessagesTests
{
	public class Test
	{
		private const string Username = "user1234";
		private const string NewEmail = "new@gmail.com";
		private readonly string _userId = new Guid().ToString();
		private readonly string _verificationCode = new Guid().ToString();

		[Fact]
		public void WelcomeEmailHtml()
		{
			var options = OptionsMockFactory.EmailOptions();
			var emailMessages = new EmailMessages(options);
			var text = emailMessages.WelcomeEmailHtml(Username, _userId, _verificationCode);

			Assert.Contains(Username, text);
			Assert.Contains(_userId, text);
			Assert.Contains(_verificationCode, text);
		}

		[Fact]
		public void WelcomeEmailText()
		{
			var options = OptionsMockFactory.EmailOptions();
			var emailMessages = new EmailMessages(options);
			var text = emailMessages.WelcomeEmailText(Username, _userId, _verificationCode);

			Assert.Contains(Username, text);
			Assert.Contains(_userId, text);
			Assert.Contains(_verificationCode, text);
		}

		[Fact]
		public void ChangeEmailHtml()
		{
			var options = OptionsMockFactory.EmailOptions();
			var emailMessages = new EmailMessages(options);
			var text = emailMessages.ChangeEmailHtml(Username, NewEmail, _userId, _verificationCode);

			Assert.Contains(Username, text);
			Assert.Contains(NewEmail, text);
			Assert.Contains(_userId, text);
			Assert.Contains(_verificationCode, text);
		}

		[Fact]
		public void ChangeEmailText()
		{
			var options = OptionsMockFactory.EmailOptions();
			var emailMessages = new EmailMessages(options);
			var text = emailMessages.ChangeEmailText(Username, NewEmail, _userId, _verificationCode);

			Assert.Contains(Username, text);
			Assert.Contains(NewEmail, text);
			Assert.Contains(_userId, text);
			Assert.Contains(_verificationCode, text);
		}

		[Fact]
		public void ResetPasswordHtml()
		{
			var options = OptionsMockFactory.EmailOptions();
			var emailMessages = new EmailMessages(options);
			var text = emailMessages.ResetPasswordHtml(Username, _userId, _verificationCode);

			Assert.Contains(Username, text);
			Assert.Contains(_userId, text);
			Assert.Contains(_verificationCode, text);
		}

		[Fact]
		public void ResetPasswordText()
		{
			var options = OptionsMockFactory.EmailOptions();
			var emailMessages = new EmailMessages(options);
			var text = emailMessages.ResetPasswordText(Username, _userId, _verificationCode);

			Assert.Contains(Username, text);
			Assert.Contains(_userId, text);
			Assert.Contains(_verificationCode, text);
		}
	}
}