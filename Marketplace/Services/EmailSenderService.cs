using MailKit.Security;
using MimeKit;

namespace Marketplace.Services
{
    public class EmailSenderService(IConfiguration _config) : IEmailSenderService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_config["Email:User"], _config["Email:Password"]);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("MonApp", _config["Email:User"]));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}