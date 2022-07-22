using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;

namespace Palavyr.API.Registration.Container
{
    public static class ServiceRegistry
    {
        public static void RegisterHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks();
        }

        public static void RegisterIisConfiguration(IServiceCollection services, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment()) // only need IIS configured in production and staging
            {
                if (Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });
                }
            }
        }

        public static void RegisterDatabaseContexts(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDataContexts>(
                opt =>
                {
                    // var conString = "Server=localhost;Port=5432;Database=AppDatabase;User Id=postgres;Password=Password01!";
                    opt.UseNpgsql(configuration.CorrectConnectionString());
                });
        }
    }
}