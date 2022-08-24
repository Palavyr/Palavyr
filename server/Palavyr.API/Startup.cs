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
        private ConfigurationContainer configuration;
        private readonly IWebHostEnvironment env;

        public Startup(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public ILifetimeScope AutofacContainer { get; private set; } = null!;

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            ContainerSetup(builder, configuration);
        }

        public static void ContainerSetup(ContainerBuilder builder, ConfigurationContainer configuration)
        {
            builder.RegisterModule(new AmazonModule(configuration));
            builder.RegisterModule(new GeneralModule());
            builder.RegisterModule(new StripeModule(configuration));
            builder.RegisterInstance(configuration).AsSelf();
        }

        public static void RegisterStores(IServiceCollection services, ConfigurationContainer configuration)
        {
            ServiceRegistry.RegisterDatabaseContexts(services, configuration);
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            configuration = ConfigurationGetter.GetConfiguration();
            AuthenticationConfiguration.AddAuthenticationSchemes(services, configuration);
            SetServices(services, configuration, env);
        }


        public static void SetServices(IServiceCollection services, ConfigurationContainer config, IWebHostEnvironment environ)
        {
            services.AddHttpContextAccessor();
            services.AddControllers().AddControllersAsServices();
            services.AddAuthentication().AddCertificate();

            CorsConfiguration.ConfigureCorsService(services, environ);
            ServiceRegistry.RegisterIisConfiguration(services, environ);
            NonWebHostConfiguration(services, config);
        }

        public static void NonWebHostConfiguration(IServiceCollection services, ConfigurationContainer config)
        {
            RegisterStores(services, config);
            ServiceRegistry.RegisterHealthChecks(services);
            MediatorRegistry.RegisterMediator(services);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            PalavyrAccessChecker.AssertEnvironmentsDoNoOverlap();

            app.UseHttpsRedirection();

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