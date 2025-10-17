using Marketplace.Application.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Marketplace.Application.Dto.GestionComptesDto;

namespace Marketplace.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GestionComptesController : ControllerBase
    {
        private readonly IGestionComptesService _gestionComptesService;

        public GestionComptesController(IGestionComptesService gestionComptesService)
        {
            _gestionComptesService = gestionComptesService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            var baseUrl = Url.Action("ConfirmEmail", "GestionComptes", null, Request.Scheme);
            var (success, message) = await _gestionComptesService.RegisterAsync(request, baseUrl!);
            return success ? Ok(message) : BadRequest(message);
        }

        [HttpGet("Confirm")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var (success, message) = await _gestionComptesService.ConfirmEmailAsync(userId, token);
            return success ? Ok(message) : BadRequest(message);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var (success, token, message) = await _gestionComptesService.LoginAsync(dto);
            if (!success)
                return Unauthorized(message);

            return Ok(new LoginResponseDto { token = token });
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _gestionComptesService.LogoutAsync();
            return Ok();
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}/reset-password.html";
            await _gestionComptesService.ForgotPasswordAsync(request, baseUrl);
            return Ok("If the email exists, a reset link will be sent.");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            var (success, message) = await _gestionComptesService.ResetPasswordAsync(request);
            return success ? Ok(message) : BadRequest(message);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Secret")]
        public IActionResult GetSecret() => Ok("Bienvenue Admin 👑 !");

        [HttpGet("Ping")]
        public IActionResult Ping() => Ok("Pong ✅ (utilisateur connecté)");
    }
}
