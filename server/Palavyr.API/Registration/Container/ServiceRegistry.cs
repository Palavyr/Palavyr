using System;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Palavyr.Core.Data;
using Palavyr.Core.GlobalConstants;

namespace Palavyr.API.Registration.Container
{
    public static class ServiceRegistry
    {

        public static void RegisterMediator(IServiceCollection serviceCollection)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            // var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains("Palavyr.API") || x.FullName.Contains("Palavyr.Core"));
            serviceCollection.AddMediatR(assemblies.ToArray());
        }

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
            services.AddDbContext<AccountsContext>(
                opt =>
                    opt.UseNpgsql(configuration.GetConnectionString(ApplicationConstants.ConfigSections.AccountDbStringKey)));
            services.AddDbContext<ConvoContext>(
                opt =>
                    opt.UseNpgsql(configuration.GetConnectionString(ApplicationConstants.ConfigSections.ConvoDbStringKey)));
            services.AddDbContext<DashContext>(
                opt =>
                    opt.UseNpgsql(configuration.GetConnectionString(ApplicationConstants.ConfigSections.ConfigurationDbStringKey)));
        }
    }
}