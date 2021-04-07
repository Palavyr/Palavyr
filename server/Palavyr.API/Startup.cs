using Autofac;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Palavyr.API.CustomMiddleware;
using Palavyr.API.Registration.BackgroundJobs;
using Palavyr.API.Registration.Configuration;
using Palavyr.API.Registration.Container;

namespace Palavyr.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            this.env = env;
            this.configuration = configuration;
        }

        public ILifetimeScope AutofacContainer { get; private set; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AmazonModule(configuration));
            builder.RegisterModule(new HangfireModule());
            builder.RegisterModule(new GeneralModule());
            builder.RegisterModule(new StripeModule(configuration));
            builder.RegisterModule(new RepositoriesModule());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddLogging(loggingBuilder => { loggingBuilder.AddSeq(); });
            services.AddHttpContextAccessor();

            AuthenticationConfiguration.AddAuthenticationSchemes(services, configuration);
            CorsConfiguration.ConfigureCorsService(services, env);
            services.AddControllers();

            Configurations.ConfigureStripe(configuration);
            ServiceRegistry.RegisterDatabaseContexts(services, configuration);
            ServiceRegistry.RegisterHealthChecks(services);
            ServiceRegistry.RegisterHangfire(services, env);
        }

        public void Configure(
            IApplicationBuilder app,
            ILoggerFactory loggerFactory,
            HangFireJobs hangFireJobs
        )
        {
            // app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseRequestResponseLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<SetHeadersMiddleware>(); // MUST come after UseAuthentication to ensure we are setting these headers on authenticated requests
            hangFireJobs.AddHangFireJobs(app);
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHangfireDashboard();
                    endpoints.MapHealthChecks("/healthcheck");
                });
        }
    }
}