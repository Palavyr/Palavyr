using System;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Palavyr.BackupAndRestore;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.BackupAndRestore.UserData;
using Palavyr.Core.Common.GlobalConstants;
using Palavyr.Core.Data;

namespace Palavyr.API.Registration.Container
{
    public static class ServiceRegistry
    {

        public static void RegisterHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks();
        }

        public static void RegisterHangfire(IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddHangfire(
                config =>
                    config
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseMemoryStorage());
            services.AddHangfireServer(
                opt =>
                {
                    opt.WorkerCount = 1;
                });
        }

        public static void RegisterIISConfiguration(IServiceCollection services, IWebHostEnvironment env)
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
            services.AddDbContext<AccountsContext>(
                opt =>
                    opt.UseNpgsql(configuration.GetConnectionString(ConfigSections.AccountDbStringKey)));
            services.AddDbContext<ConvoContext>(
                opt =>
                    opt.UseNpgsql(configuration.GetConnectionString(ConfigSections.ConvoDbStringKey)));
            services.AddDbContext<DashContext>(
                opt =>
                    opt.UseNpgsql(configuration.GetConnectionString(ConfigSections.ConfigurationDbStringKey)));
        }
    }
}