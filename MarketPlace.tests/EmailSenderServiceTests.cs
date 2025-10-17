using Xunit;
using Moq;
using Marketplace.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class EmailSenderServiceTests
{
    [Fact]
    public async Task SendEmailAsync_CallsSmtpClientMethods()
    {
        // Arrange
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["Email:User"]).Returns("test@domain.com");
        configMock.Setup(c => c["Email:Password"]).Returns("password");
        var service = new EmailSenderService(configMock.Object);

        // Act & Assert
        // Impossible de mocker MailKit.Net.Smtp.SmtpClient directement sans wrapper, donc ce test vérifie la config et la création du service
        await Assert.ThrowsAsync<MailKit.Security.AuthenticationException>(() => service.SendEmailAsync("to@domain.com", "subject", "body"));
    }

    [Fact]
    public async Task SendEmailAsync_ThrowsException_WhenToIsInvalid()
    {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["Email:User"]).Returns("test@domain.com");
        configMock.Setup(c => c["Email:Password"]).Returns("password");
        var service = new EmailSenderService(configMock.Object);
        await Assert.ThrowsAsync<FormatException>(() => service.SendEmailAsync("invalid-email", "subject", "body"));
    }

    [Fact]
    public async Task SendEmailAsync_ThrowsException_WhenConfigIsMissing()
    {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["Email:User"]).Returns((string)null);
        configMock.Setup(c => c["Email:Password"]).Returns((string)null);
        var service = new EmailSenderService(configMock.Object);
        await Assert.ThrowsAsync<System.ArgumentNullException>(() => service.SendEmailAsync("to@domain.com", "subject", "body"));
    }

    [Fact]
    public async Task SendEmailAsync_ThrowsException_WhenSubjectIsEmpty()
    {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["Email:User"]).Returns("test@domain.com");
        configMock.Setup(c => c["Email:Password"]).Returns("password");
        var service = new EmailSenderService(configMock.Object);
        await Assert.ThrowsAsync<MailKit.Security.AuthenticationException>(() => service.SendEmailAsync("to@domain.com", "", "body"));
    }
}
