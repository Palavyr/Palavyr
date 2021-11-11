#nullable enable
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Palavyr.API.CustomMiddleware;
using Palavyr.API.Registration.Configuration;
using Palavyr.API.Registration.Container;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.API
{
    public class Startup
    {
        
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;
        private readonly ErrorHandler errorHandler = new ErrorHandler();

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            this.env = env;
            this.configuration = configuration;
        }

        public ILifetimeScope AutofacContainer { get; private set; } = null!;

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            ContainerSetup(builder, configuration);
        }

        public static void ContainerSetup(ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterModule(new AmazonModule(configuration));
            builder.RegisterModule(new GeneralModule());
            builder.RegisterModule(new StripeModule(configuration));
            builder.RegisterModule(new RepositoriesModule());
        }

        public static void RegisterStores(IServiceCollection services, IConfiguration configuration)
        {
            ServiceRegistry.RegisterDatabaseContexts(services, configuration);
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            AuthenticationConfiguration.AddAuthenticationSchemes(services, configuration);
            SetServices(services, configuration, env);
        }

        public static void SetServices(IServiceCollection services, IConfiguration config, IWebHostEnvironment environ)
        {
            services.AddHttpContextAccessor();
            CorsConfiguration.ConfigureCorsService(services, environ);
            services.AddControllers();
            Configurations.ConfigureStripe(config);
            RegisterStores(services, config);
            ServiceRegistry.RegisterHealthChecks(services);
            ServiceRegistry.RegisterIisConfiguration(services, environ);
        }

        public void Configure(
            IApplicationBuilder app,
            ILoggerFactory loggerFactory
        )
        {
            PalavyrAccessChecker.AssertEnvironmentsDoNoOverlap();
            
            // var logger = loggerFactory.CreateLogger("Error Handler");
            // app.UseRequestResponseLogging(); // THIS STUPID THING IS DISPOSING THE RESPONSE BODY!!!
            app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseCors();
            // app.UseExceptionHandler(
            //     errorApp =>
            //     {
            //         errorApp.Run(
            //             async context =>
            //             {
            //                 await errorHandler.HandleErrors(context, logger);
            //             });
            //     });
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<SetHeadersMiddleware>(); // MUST come after UseAuthentication to ensure we are setting these headers on authenticated requests


            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHealthChecks("/healthcheck");
                });
        }
    }
}