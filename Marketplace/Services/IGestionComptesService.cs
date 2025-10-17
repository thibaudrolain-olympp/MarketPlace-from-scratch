using static Marketplace.Dto.GestionComptesDto;

namespace Marketplace.Business.Interfaces
{
    public interface IGestionComptesService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterUserDto request, string confirmBaseUrl);
        Task<(bool Success, string Message)> ConfirmEmailAsync(string userId, string token);
        Task<(bool Success, string Token, string? Message)> LoginAsync(LoginDto dto);
        Task ForgotPasswordAsync(ForgotPasswordRequestDto request, string resetBaseUrl);
        Task<(bool Success, string Message)> ResetPasswordAsync(ResetPasswordRequestDto request);
        Task LogoutAsync();
    }
}
