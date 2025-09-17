using Marketplace.Controllers;
using Marketplace.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Marketplace.Dto.GestionComptesDto;

namespace Marketplace.Tests.Controllers
{
    /// <summary>
    /// Classe de tests unitaires pour le contrôleur GestionComptesController.
    /// Ces tests vérifient les principaux scénarios de gestion des comptes utilisateurs :
    /// inscription, connexion, confirmation d'email, réinitialisation du mot de passe, etc.
    /// </summary>
    public class GestionComptesControllerTests
    {
        // Mocks des dépendances du contrôleur
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IEmailSenderService> _emailSenderMock;

        // Instance du contrôleur à tester
        private readonly GestionComptesController _controller;

        /// <summary>
        /// Constructeur : initialise les mocks et le contrôleur avec ses dépendances.
        /// </summary>
        public GestionComptesControllerTests()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                _userManagerMock.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null, null, null, null);

            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);

            _configMock = new Mock<IConfiguration>();
            _emailSenderMock = new Mock<IEmailSenderService>();

            _controller = new GestionComptesController(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _roleManagerMock.Object,
                _configMock.Object,
                _emailSenderMock.Object
            );

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns("http://localhost/confirm");
            _controller.Url = urlHelperMock.Object;

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.ControllerContext.HttpContext.Request.Scheme = "http";
            _controller.ControllerContext.HttpContext.Request.Host = new HostString("localhost");
        }


        // ------------------- REGISTER -------------------
        /// <summary>
        /// Test : Vérifie que l'inscription retourne Ok quand tout se passe bien.
        /// </summary>
        [Fact]
        public async Task Register_Should_Return_Ok_When_Success()
        {
            // Arrange : prépare le DTO et les mocks pour simuler une inscription réussie
            var dto = new RegisterUserDto { Email = "new@test.com", Username = "newuser", Password = "P@ssw0rd" };

            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser)null);
            _userManagerMock.Setup(x => x.FindByNameAsync(dto.Username))
                .ReturnsAsync((IdentityUser)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(x => x.RoleExistsAsync("User"))
                .ReturnsAsync(false);
            _roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "User"))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync("confirmation-token");

            // Act : appelle la méthode Register
            var result = await _controller.Register(dto);

            // Assert : vérifie le type et le contenu du résultat
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User registered. Please confirm your email.", ok.Value);
            _emailSenderMock.Verify(x => x.SendEmailAsync(dto.Email, "Confirmer votre compte", It.IsAny<string>()), Times.Once);
        }

        // ------------------- LOGIN -------------------
        /// <summary>
        /// Test : Vérifie que la connexion retourne un token JWT valide.
        /// </summary>
        [Fact]
        public async Task Login_Should_Return_Token_When_Success()
        {
            // Arrange : prépare le DTO et les mocks pour simuler une connexion réussie
            var user = new IdentityUser { UserName = "user1", EmailConfirmed = true };
            var dto = new LoginDto { Username = "user1", Password = "pwd" };

            _userManagerMock.Setup(x => x.FindByNameAsync(dto.Username))
                .ReturnsAsync(user);
            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, dto.Password, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            _configMock.Setup(x => x["JwtSettings:Key"]).Returns("ThisIsASuperSecretKeyForJwt1234567890!!!"); // 32+ chars
            _configMock.Setup(x => x["JwtSettings:Issuer"]).Returns("test-issuer");
            _configMock.Setup(x => x["JwtSettings:Audience"]).Returns("test-audience");

            // Act : appelle la méthode Login
            var result = await _controller.Login(dto);

            // Assert : vérifie que le token est bien généré et décodable
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginResponseDto>(ok.Value);
            Assert.False(string.IsNullOrEmpty(response.token));

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(response.token);
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Name && c.Value == user.UserName);
        }

        // ------------------- CONFIRM EMAIL -------------------
        /// <summary>
        /// Test : Vérifie que la confirmation d'email retourne Ok si le token est valide.
        /// </summary>
        [Fact]
        public async Task ConfirmEmail_Should_Return_Ok_When_Token_Is_Valid()
        {
            // Arrange : prépare le user et le token
            var user = new IdentityUser { Id = "123", Email = "test@test.com" };
            var rawToken = "confirmation-token";
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawToken));

            _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(user, rawToken))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.ConfirmEmail(user.Id, encodedToken);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Email confirmed successfully!", ok.Value);
        }

        // ------------------- CONFIRM EMAIL (FAIL CASES) -------------------
        /// <summary>
        /// Test : Vérifie que la confirmation d'email retourne BadRequest si l'utilisateur n'existe pas.
        /// </summary>
        [Fact]
        public async Task ConfirmEmail_Should_Return_BadRequest_When_User_Not_Found()
        {
            // Arrange
            var userId = "not-exist";
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("dummy-token"));
            _userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync((IdentityUser)null);

            // Act
            var result = await _controller.ConfirmEmail(userId, encodedToken);

            // Assert
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid user.", bad.Value);
        }

        /// <summary>
        /// Test : Vérifie que la confirmation d'email retourne BadRequest si le token est invalide.
        /// </summary>
        [Fact]
        public async Task ConfirmEmail_Should_Return_BadRequest_When_Token_Invalid()
        {
            // Arrange
            var user = new IdentityUser { Id = "123", Email = "test@test.com" };
            var rawToken = "wrong-token";
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawToken));

            _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(user, rawToken))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid token" }));

            // Act
            var result = await _controller.ConfirmEmail(user.Id, encodedToken);

            // Assert
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid token.", bad.Value);
        }

        // ------------------- RESET PASSWORD (FAIL CASES) -------------------
        /// <summary>
        /// Test : Vérifie que la réinitialisation du mot de passe retourne BadRequest si l'utilisateur n'existe pas.
        /// </summary>
        [Fact]
        public async Task ResetPassword_Should_Return_BadRequest_When_User_Not_Found()
        {
            // Arrange
            var dto = new ResetPasswordRequestDto
            {
                Email = "unknown@test.com",
                Token = "tok",
                NewPassword = "newpass"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser)null);

            // Act
            var result = await _controller.ResetPassword(dto);

            // Assert
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request.", bad.Value);
        }

        /// <summary>
        /// Test : Vérifie que la réinitialisation du mot de passe retourne BadRequest si la réinitialisation échoue.
        /// </summary>
        [Fact]
        public async Task ResetPassword_Should_Return_BadRequest_When_Reset_Fails()
        {
            // Arrange
            var user = new IdentityUser { Email = "test@test.com" };
            var dto = new ResetPasswordRequestDto
            {
                Email = user.Email,
                Token = "tok",
                NewPassword = "newpass"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.ResetPasswordAsync(user, It.IsAny<string>(), dto.NewPassword))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password too weak" }));

            // Act
            var result = await _controller.ResetPassword(dto);

            // Assert
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsAssignableFrom<IEnumerable<IdentityError>>(bad.Value);
            Assert.Contains(errors, e => e.Description == "Password too weak");
        }

        // ------------------- LOGOUT -------------------
        /// <summary>
        /// Test : Vérifie que la déconnexion appelle bien SignOut et retourne Ok.
        /// </summary>
        [Fact]
        public async Task Logout_Should_Call_SignOut()
        {
            // Act
            var result = await _controller.Logout();

            // Assert
            Assert.IsType<OkResult>(result);
            _signInManagerMock.Verify(x => x.SignOutAsync(), Times.Once);
        }

        // ------------------- SECRET -------------------
        /// <summary>
        /// Test : Vérifie que l'endpoint secret retourne Ok pour un admin.
        /// </summary>
        [Fact]
        public void Secret_Should_Return_Ok_For_Admin()
        {
            // Act
            var result = _controller.GetSecret();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Bienvenue Admin 👑 !", ok.Value);
        }

        // ------------------- PING -------------------
        /// <summary>
        /// Test : Vérifie que l'endpoint ping retourne Pong.
        /// </summary>
        [Fact]
        public void Ping_Should_Return_Pong()
        {
            // Act
            var result = _controller.Ping();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Pong ✅ (utilisateur connecté)", ok.Value);
        }
    }
}