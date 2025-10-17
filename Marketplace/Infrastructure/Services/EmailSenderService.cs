using MailKit.Security;
using Marketplace.Application.ServicesInterfaces;
using MimeKit;

namespace Marketplace.Infrastructure.Services
{
    public class EmailSenderService(IConfiguration _config) : IEmailSenderService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Validate configuration first
            var user = _config["Email:User"];
            var password = _config["Email:Password"];
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Email configuration is missing or incomplete.");

            // Validate recipient address before attempting SMTP authentication
            if (!MailboxAddress.TryParse(to, out var mailbox))
                throw new FormatException("The 'to' email address is not a valid mailbox address.");

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(user, password);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("MonApp", user));
            message.To.Add(mailbox);
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}