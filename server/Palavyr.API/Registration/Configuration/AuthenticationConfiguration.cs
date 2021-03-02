using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Palavyr.API.CustomMiddleware;
using Palavyr.Common.GlobalConstants;
using Palavyr.Services.AuthenticationServices;

//https://adamstorr.azurewebsites.net/blog/integration-testing-with-aspnetcore-3-1-remove-the-boiler-plate
namespace Palavyr.API.Registration.Configuration
{
    public static class AuthenticationConfiguration
    {
        public static void AddAuthenticationSchemes(IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration[ConfigSections.JwtSecretKey] ?? throw new ArgumentNullException("Configuration[\"JWTSecretKey\"]");

            services
                .AddAuthentication(
                    o =>
                    {
                        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(
                    opt =>
                    {
                        opt.RequireHttpsMetadata = true;
                        opt.SaveToken = true;

                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            // ValidateLifetime = false,
                            // ValidIssuer = Configuration["JwtToken:Issuer"],
                            // ValidAudience = Configuration["JwtToken:Issuer"],
                        };
                    })
                .AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthenticationHandler>(
                    AuthenticationSchemeNames.ApiKeyScheme,
                    op => { });
        }
    }
}