using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Palavyr.API.CustomMiddleware;
using Palavyr.Core.Configuration;
using Palavyr.Core.Services.AuthenticationServices;

//https://adamstorr.azurewebsites.net/blog/integration-testing-with-aspnetcore-3-1-remove-the-boiler-plate
namespace Palavyr.API.Registration.Configuration
{
    public class JwtTokenConfig
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }


    public static class AuthenticationConfiguration
    {
        public static void AddAuthenticationSchemes(IServiceCollection services, ConfigContainerServer config)
        {
            // var jwtTokenConfig = configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
            //https://wildermuth.com/2018/04/10/Using-JwtBearer-Authentication-in-an-API-only-ASP-NET-Core-Project
            // services.AddIdentityCore<IdentityUser>(
            //     cfg =>
            //     {
            //         cfg.User.RequireUniqueEmail = true;
            //     });

            var key = config.JwtSecretKey;
            
            services
                .AddAuthentication(
                    o =>
                    {
                        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer("Bearer",
                    opt =>
                    {
                        opt.RequireHttpsMetadata = false;
                        opt.SaveToken = true;
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            // ValidateIssuer = true,
                            // ValidIssuer = jwtTokenConfig.Issuer,
                            // ValidateIssuerSigningKey = true,
                            // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenConfig.Secret)),
                            // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                            // ValidAudience = jwtTokenConfig.Audience,
                            // ValidateAudience = true,
                            // ValidateLifetime = true,
                            // ClockSkew = TimeSpan.FromMinutes(1)
                        };
                    })
                .AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthenticationHandler>(
                    AuthenticationSchemeNames.ApiKeyScheme,
                    op => { });
        }
    }
}