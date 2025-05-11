using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SmartLink.Host.Web.Models;

namespace SmartLink.Host.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<AuthResult> LoginAsync(string username, string password)
        {
            // In a real application, you would validate against a database
            // This is just a placeholder implementation
            if (username == "admin" && password == "password")
            {
                var token = GenerateJwtToken(username);
                return new AuthResult
                {
                    Success = true,
                    Token = token,
                    Message = "Login successful"
                };
            }

            return new AuthResult
            {
                Success = false,
                Message = "Invalid username or password"
            };
        }

        public async Task<AuthResult> RegisterAsync(string username, string email, string password)
        {

            return new AuthResult
            {
                Success = true,
                Message = "Registration successful"
            };
        }

        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
