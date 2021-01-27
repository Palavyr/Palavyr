using Autofac;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Palavyr.API.CustomMiddleware;
using Palavyr.API.Registration.Application;
using Palavyr.API.Registration.Autofac;
using Palavyr.API.Registration.ServiceCollection;

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
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => { loggingBuilder.AddSeq(); });
            services.AddHttpContextAccessor();
            
            AuthenticationConfiguration.AddAuthenticationSchemes(services, configuration);
            
            CorsConfiguration.AddCors(services, env);
            services.AddControllers();
            
            Configurations.ConfigureStripe(configuration);
            ServiceRegistry.RegisterDatabaseContexts(services, configuration);
            ServiceRegistry.RegisterBackgroundServices(services);
            ServiceRegistry.RegisterGeneralServices(services);
            ServiceRegistry.RegisterHangfire(services, env);
        }

        public void Configure(
            IApplicationBuilder app,
            ILoggerFactory loggerFactory,
            HangFireJobs hangFireJobs
            )
        {
            var logger = loggerFactory.CreateLogger<Startup>();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<SetHeaders>(); // MUST come after UseAuthentication to ensure we are setting these headers on authenticated requests
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHangfireDashboard();
                });
            hangFireJobs.AddHangFireJobs(app);
        }
    }
}