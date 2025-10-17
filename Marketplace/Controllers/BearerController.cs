using Marketplace.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BearerController(IConfiguration configuration) : ControllerBase
    {
        [HttpPost(Name = "Token")]
        public ActionResult GetToken([FromBody] BearerRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Key) || request.Key != configuration.GetSection("JwtSettings")["Key"])
            {
                return new UnauthorizedResult();
            }

            // Normally, you would generate a JWT token here and return it.
            var cle = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings")["Key"]));

            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(cle, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                /*                issuer: configuration.GetSection("JwtSettings")["Issuer"],
                                audience: configuration.GetSection("JwtSettings")["Audience"],*/
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddMinutes(30)
            );

            var jwtHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);

            return new OkObjectResult(new { token = jwtHandler });
        }
    }
}