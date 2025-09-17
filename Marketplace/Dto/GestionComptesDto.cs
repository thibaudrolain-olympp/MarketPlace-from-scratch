namespace Marketplace.Dto
{
    public class GestionComptesDto
    {
        public class LoginDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class LoginResponseDto
        {
            public string token { get; set; }
        }
        public class RegisterUserDto
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
        }

        public class ConfirmEmailRequestDto
        {
            public string Email { get; set; }
            public string Token { get; set; }
        }

        public class ForgotPasswordRequestDto
        {
            public string Email { get; set; }
        }

        public class ResetPasswordRequestDto
        {
            public string Email { get; set; }
            public string Token { get; set; }
            public string NewPassword { get; set; }
        }
    }
}