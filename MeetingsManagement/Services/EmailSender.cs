using System.Net.Mail;
using System.Net;

namespace MeetingsManagementWeb.Services
{
    public class EmailSender
    {
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        private readonly SmtpClient _smtpClient;
        public EmailSender() {
            _senderEmail = Environment.GetEnvironmentVariable("EMAIL_SENDER_EMAIL")!;
            _senderPassword = Environment.GetEnvironmentVariable("EMAIL_SENDER_PASSWORD")!;
            _smtpClient = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                Credentials = new NetworkCredential(_senderEmail, _senderPassword),
                EnableSsl = true
            };
        }

        public void Send(string message, string receiverEmail)
        {
            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_senderEmail, "My System Sender"),
                Subject = "Gentle Reminder",
                Body = message
            };
            mailMessage.To.Add(receiverEmail);
            _smtpClient.Send(mailMessage);
        }
    }
}
