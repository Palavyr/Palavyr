using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Palavyr.Core.Configuration;
using Palavyr.Core.Data;
using Serilog;

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

        public static void RegisterDatabaseContexts(IServiceCollection services, ConfigContainerServer config)
        {
            services.AddDbContext<AppDataContexts>(
                opt =>
                {
                    // var conString = "Server=localhost;Port=5432;Database=AppDatabase;User Id=postgres;Password=Password01!";
                    Console.WriteLine("=====================================");
                    Console.WriteLine($"{config.DbConnectionString}");
                    Console.WriteLine("=====================================");
                    
                    Log.Debug("=====================================");
                    Log.Debug("{ConnectionString}", config.DbConnectionString);
                    Log.Debug("=====================================");
                    opt.UseNpgsql(config.DbConnectionString);
                });
        }
    }
}