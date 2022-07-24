using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Palavyr.Core.Common.ExtensionMethods;

namespace Palavyr.Core.Services.AuthenticationServices
{
    public interface IJwtAuthenticationService
    {
        string GenerateJwtTokenAfterAuthentication(string username);
    }

    public class JwtAuthenticationService : IJwtAuthenticationService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<JwtAuthenticationService> logger;

        public JwtAuthenticationService(IConfiguration configuration, ILogger<JwtAuthenticationService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public string GenerateJwtTokenAfterAuthentication(string username)
        {
            logger.LogDebug("Creating JWT token... ");
            var key = configuration.GetJwtKey();//["JWT:SecretKey"] ?? throw new ArgumentNullException("Configuration[\"JWTSecretKey\"]");

            logger.LogDebug("Using the JWT SecretKey: {key}");
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username),
                    }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            logger.LogDebug($"Issuing the token: {token}");

            var written = tokenHandler.WriteToken(token);
            logger.LogDebug($"Using the written token: {written}");
            return written;
        }
    }
}