using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Palavyr.API.Controllers
{
    public interface IJwtAuthenticationService
    {
        string GenerateJwtTokenAfterAuthentication(string username);
    }
    
    public class JwtAuthenticationService : IJwtAuthenticationService
    {
        private readonly IConfiguration _configuration;
        public JwtAuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtTokenAfterAuthentication(string username)
        {
            var key = _configuration["JWTSecretKey"] ?? throw new ArgumentNullException("Configuration[\"JWTSecretKey\"]");
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username), 
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256Signature )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var written = tokenHandler.WriteToken(token);
            return written;
        }
    }
}