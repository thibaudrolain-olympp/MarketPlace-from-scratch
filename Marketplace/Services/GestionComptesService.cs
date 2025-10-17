using Marketplace.Business.Interfaces;
using Marketplace.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Marketplace.Dto.GestionComptesDto;

namespace Marketplace.Business.Services
{
    public class GestionComptesService : IGestionComptesService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IEmailSenderService _emailSender;

        public GestionComptesService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config,
            IEmailSenderService emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _emailSender = emailSender;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterUserDto request, string confirmBaseUrl)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return (false, "This email is already registered.");

            var existingUsername = await _userManager.FindByNameAsync(request.Username);
            if (existingUsername != null)
                return (false, "This username is already taken.");

            var user = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var confirmLink = $"{confirmBaseUrl}?userId={user.Id}&token={encodedToken}";
            var emailBody = $@"
                <p>Cliquez sur le bouton ci-dessous pour confirmer votre email :</p>
                <a href='{confirmLink}' style='display:inline-block;padding:10px 20px;background-color:#28a745;color:white;text-decoration:none;border-radius:5px;'>Confirmer mon email</a>";

            await _emailSender.SendEmailAsync(user.Email, "Confirmer votre compte", emailBody);

            return (true, "User registered. Please confirm your email.");
        }

        public async Task<(bool Success, string Message)> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "Invalid user.");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            return result.Succeeded ? (true, "Email confirmed successfully!") : (false, "Invalid token.");
        }

        public async Task<(bool Success, string Token, string? Message)> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null)
                return (false, "", "Unauthorized");

            if (!user.EmailConfirmed)
                return (false, "", "Please confirm your email first.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return (false, "", "Invalid credentials");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return (true, new JwtSecurityTokenHandler().WriteToken(token), null);
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequestDto request, string resetBaseUrl)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);

            var resetLink = $"{resetBaseUrl}?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(encodedToken)}";

            var emailBody = $@"
                <p>Cliquez sur le bouton ci-dessous pour réinitialiser votre mot de passe :</p>
                <a href='{resetLink}' style='display:inline-block;padding:10px 20px;background-color:#007bff;color:white;text-decoration:none;border-radius:5px;'>Réinitialiser</a>";

            await _emailSender.SendEmailAsync(user.Email, "Réinitialiser le mot de passe", emailBody);
        }

        public async Task<(bool Success, string Message)> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return (false, "Invalid request.");

            var decodedToken = System.Web.HttpUtility.UrlDecode(request.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

            return result.Succeeded ? (true, "Password has been reset successfully!") : (false, "Invalid token or password.");
        }

        public async Task LogoutAsync() => await _signInManager.SignOutAsync();
    }
}
