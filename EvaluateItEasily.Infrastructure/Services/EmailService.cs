using EvaluateItEasily.Core.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
namespace EvaluateItEasily.Infrastructure.Services
{
    public class EmailService(IOptions<EmailSettings> options) : IEmailSender
    {
        private readonly EmailSettings _mailSettings = options.Value;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail),
                Subject = subject
            };
            message.To.Add(MailboxAddress.Parse(email));

            var builder = new BodyBuilder { HtmlBody = htmlMessage };
            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(quit: true);
        }
    }
}
