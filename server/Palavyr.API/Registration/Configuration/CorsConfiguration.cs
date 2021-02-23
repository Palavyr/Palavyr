using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Palavyr.API.Registration.Configuration
{
    public static class CorsConfiguration
    {
        public static void AddCors(IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddCors(
                options =>
                {
                    options.AddDefaultPolicy(
                        builder =>
                        {
                            builder
                                .SetIsOriginAllowed(_ => true)
                                .WithMethods("DELETE", "POST", "GET", "OPTIONS", "PUT")
                                .WithHeaders(
                                    "action",
                                    "Server",
                                    "sessionId",
                                    "Content-Type",
                                    "Access-Control-Allow-Origin",
                                    "Access-Control-Allow-Headers",
                                    "Access-Control-Allow-Methods",
                                    "Authorization",
                                    "X-Requested-With"
                                );

                            if (env.IsDevelopment() || env.IsStaging())
                            {
                                builder.WithOrigins("*");
                            }
                            else
                            {
                                builder.WithOrigins(
                                    "http://staging.palavyr.com",
                                    "http://www.staging.palavyr.com",
                                    "http://palavyr.com",
                                    "http://www.palavyr.com",
                                    "http://staging.widget.palavyr.com",
                                    "http://widget.palavyr.com",
                                    "https://staging.palavyr.com",
                                    "https://www.staging.palavyr.com",
                                    "https://palavyr.com",
                                    "https://www.palavyr.com",
                                    "https://staging.widget.palavyr.com",
                                    "https://widget.palavyr.com",
                                    "https://stripe.com"
                                );
                            }
                        });
                });
        }
    }
}