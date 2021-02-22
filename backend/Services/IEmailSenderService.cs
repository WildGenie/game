using System.Threading.Tasks;
using SendGrid.Helpers.Mail;

namespace Services
{
	public interface IEmailSenderService
	{
		Task SendWelcomeEmail(string username, string email, string userId, string verificationCode);
		Task SendEmailChangeConfirmationEmail(string username, string email, string userId, string verificationCode);
		Task SendPasswordResetEmail(string username, string email, string userId, string verificationCode);
	}
}