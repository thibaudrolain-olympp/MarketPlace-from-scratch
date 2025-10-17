using Marketplace.Controllers;
using Marketplace.Business.Interfaces;
using Marketplace.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Marketplace.Tests.Controllers
{
    public class GestionComptesControllerTests
    {
        private readonly Mock<IGestionComptesService> _serviceMock;
        private readonly GestionComptesController _controller;

        public GestionComptesControllerTests()
        {
            _serviceMock = new Mock<IGestionComptesService>();
            _controller = new GestionComptesController(_serviceMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
               
                HttpContext = new DefaultHttpContext()
            };
            _controller.ControllerContext.HttpContext.Request.Scheme = "http";
            _controller.ControllerContext.HttpContext.Request.Host = new HostString("localhost");

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns("http://localhost/confirm");
            _controller.Url = urlHelperMock.Object;
        }

        [Fact]
        public async Task Register_Returns_Ok_When_Service_Succeeds()
        {
            var dto = new GestionComptesDto.RegisterUserDto { Email = "a@b.com", Username = "user", Password = "P@ssw0rd" };
            _serviceMock.Setup(s => s.RegisterAsync(dto, It.IsAny<string>())).ReturnsAsync((true, "User registered. Please confirm your email."));

            var result = await _controller.Register(dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User registered. Please confirm your email.", ok.Value);
        }

        [Fact]
        public async Task Register_Returns_BadRequest_When_Service_Fails()
        {
            var dto = new GestionComptesDto.RegisterUserDto { Email = "a@b.com", Username = "user", Password = "P@ssw0rd" };
            _serviceMock.Setup(s => s.RegisterAsync(dto, It.IsAny<string>())).ReturnsAsync((false, "This email is already registered."));

            var result = await _controller.Register(dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("This email is already registered.", bad.Value);
        }

        [Fact]
        public async Task ConfirmEmail_Returns_Ok_When_Service_Succeeds()
        {
            _serviceMock.Setup(s => s.ConfirmEmailAsync("1", "tok")).ReturnsAsync((true, "Email confirmed successfully!"));

            var result = await _controller.ConfirmEmail("1", "tok");

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Email confirmed successfully!", ok.Value);
        }

        [Fact]
        public async Task ConfirmEmail_Returns_BadRequest_When_Service_Fails()
        {
            _serviceMock.Setup(s => s.ConfirmEmailAsync("1", "bad")).ReturnsAsync((false, "Invalid token."));

            var result = await _controller.ConfirmEmail("1", "bad");

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid token.", bad.Value);
        }

        [Fact]
        public async Task Login_Returns_Ok_With_Token_When_Service_Succeeds()
        {
            var dto = new GestionComptesDto.LoginDto { Username = "user", Password = "pwd" };
            _serviceMock.Setup(s => s.LoginAsync(dto)).ReturnsAsync((true, "token-value", (string?)null));

            var result = await _controller.Login(dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GestionComptesDto.LoginResponseDto>(ok.Value);
            Assert.Equal("token-value", response.token);
        }

        [Fact]
        public async Task Login_Returns_Unauthorized_When_Service_Fails()
        {
            var dto = new GestionComptesDto.LoginDto { Username = "user", Password = "pwd" };
            _serviceMock.Setup(s => s.LoginAsync(dto)).ReturnsAsync((false, "", "Unauthorized"));

            var result = await _controller.Login(dto);

            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Unauthorized", unauthorized.Value);
        }

        [Fact]
        public async Task Logout_Calls_Service_And_Returns_Ok()
        {
            _serviceMock.Setup(s => s.LogoutAsync()).Returns(Task.CompletedTask).Verifiable();

            var result = await _controller.Logout();

            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.LogoutAsync(), Times.Once);
        }

        [Fact]
        public void Secret_Returns_Ok()
        {
            var result = _controller.GetSecret();
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Bienvenue Admin 👑 !", ok.Value);
        }

        [Fact]
        public void Ping_Returns_Ok()
        {
            var result = _controller.Ping();
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Pong ✅ (utilisateur connecté)", ok.Value);
        }
    }
}