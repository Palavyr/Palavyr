using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Palavyr.API.CustomMiddleware;
using Palavyr.API.Registration.Configuration;
using Palavyr.API.Registration.Container;
using Palavyr.API.Registration.Container.MediatorModule;
using Palavyr.Core.Configuration;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.API
{
    public class Startup
    {
        private ConfigContainerServer config;
        private readonly IWebHostEnvironment env;

        public Startup(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public ILifetimeScope AutofacContainer { get; private set; } = null!;

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            ContainerSetup(builder, config);
        }

        public static void ContainerSetup(ContainerBuilder builder, ConfigContainerServer config)
        {
            builder.RegisterModule(new AmazonModule(config));
            builder.RegisterModule(new GeneralModule());
            builder.RegisterModule(new StripeModule(config));
            builder.RegisterInstance(config).AsSelf();
        }

        public static void RegisterStores(IServiceCollection services, ConfigContainerServer config)
        {
            ServiceRegistry.RegisterDatabaseContexts(services, config);
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            config = ConfigurationGetter.GetConfiguration();
            AuthenticationConfiguration.AddAuthenticationSchemes(services, config);
            SetServices(services, config, env);
        }


        public static void SetServices(IServiceCollection services, ConfigContainerServer config, IWebHostEnvironment environ)
        {
            services.AddHttpContextAccessor();
            services.AddControllers().AddControllersAsServices();
            services.AddAuthentication().AddCertificate();

            CorsConfiguration.ConfigureCorsService(services, environ);
            ServiceRegistry.RegisterIisConfiguration(services, environ);
            NonWebHostConfiguration(services, config);
        }

        public static void NonWebHostConfiguration(IServiceCollection services, ConfigContainerServer config)
        {
            RegisterStores(services, config);
            ServiceRegistry.RegisterHealthChecks(services);
            MediatorRegistry.RegisterMediator(services);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            PalavyrAccessChecker.AssertEnvironmentsDoNoOverlap();

            app.UseRouting();
            app.UseCors();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            // app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<SetCancellationTokenTransportMiddleware>();
            app.UseMiddleware<UnitOfWorkMiddleware>();
            app.UseMiddleware<SetAccountIdContextMiddleware>(); // MUST come after UseAuthentication to ensure we are setting these headers on authenticated requests

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHealthChecks("/healthcheck");
                });
        }
    }
}