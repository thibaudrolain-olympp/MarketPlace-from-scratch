namespace Marketplace.Application.ServicesInterfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}