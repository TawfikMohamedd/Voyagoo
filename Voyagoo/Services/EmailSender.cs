using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Voyagoo.Settings;

namespace Voyagoo.Services
{
    public class EmailSender(IOptions<EmailSettings> options) : IEmailSender
    {
        private readonly EmailSettings _settings = options.Value;

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            using var message = new MailMessage();
            message.From = new MailAddress(_settings.SenderEmail, _settings.SenderName);
            message.To.Add(new MailAddress(toEmail));
            message.Subject = subject;
            message.Body = htmlBody;
            message.IsBodyHtml = true;

            using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
            {
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
    }
}
