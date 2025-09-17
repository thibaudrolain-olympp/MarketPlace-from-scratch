using Marketplace.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Marketplace.Dto.GestionComptesDto;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Marketplace.Controllers
{
    [ApiController]
    /*    [Authorize]*/
    [Route("api/[controller]")]
    public class GestionComptesController(
        Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager,
        SignInManager<IdentityUser> _signInManager,
        Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager,
        IConfiguration _config, IEmailSenderService _emailSender) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            // Vérifier si l'email existe déjà
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest("This email is already registered.");

            // Vérifier si le nom d'utilisateur existe déjà (optionnel)
            var existingUsername = await _userManager.FindByNameAsync(request.Username);
            if (existingUsername != null)
                return BadRequest("This username is already taken.");
            var user = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var confirmLink = Url.Action(
                 "ConfirmEmail", "GestionComptes",
                  new { userId = user.Id, token = encodedToken }, Request.Scheme
             );

            var emailBody = $@"
<p>Cliquez sur le bouton ci-dessous pour confirmer votre email :</p>
<a href='{confirmLink}' style='display:inline-block;padding:10px 20px;background-color:#28a745;color:white;text-decoration:none;border-radius:5px;'>Confirmer mon email</a>
";

            await _emailSender.SendEmailAsync(user.Email, "Confirmer votre compte", emailBody);

            return Ok("User registered. Please confirm your email.");
        }

        [HttpGet("Confirm")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest("Invalid user.");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded) return BadRequest("Invalid token.");

            return Ok("Email confirmed successfully!");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            if (user == null)
                return Unauthorized();

            if (!user.EmailConfirmed)
                return Unauthorized("Please confirm your email first.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

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

            if (!result.Succeeded)
                return Unauthorized();

            return Ok(new LoginResponseDto
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [HttpPost("Logout")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Ok("If the email exists, a reset link will be sent."); // ne pas révéler l'existence du compte

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);

            var resetLink = $"{Request.Scheme}://{Request.Host}/reset-password.html?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(encodedToken)}";

            var emailBody = $@"
        <p>Cliquez sur le bouton ci-dessous pour réinitialiser votre mot de passe :</p>
        <a href='{resetLink}' style='display:inline-block;padding:10px 20px;background-color:#007bff;color:white;text-decoration:none;border-radius:5px;'>Réinitialiser</a>
    ";

            await _emailSender.SendEmailAsync(user.Email, "Réinitialiser le mot de passe", emailBody);

            return Ok("If the email exists, a reset link will be sent.");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest("Invalid request.");

            var decodedToken = System.Web.HttpUtility.UrlDecode(request.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password has been reset successfully!");
        }

        // Endpoint accessible uniquement par les admins
        [HttpGet("Secret")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetSecret()
        {
            return Ok("Bienvenue Admin 👑 !");
        }

        // Endpoint accessible à tout utilisateur authentifié
        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok("Pong ✅ (utilisateur connecté)");
        }
    }
}