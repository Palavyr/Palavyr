﻿
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

namespace Palavyr.IntegrationTests.AppFactory
{
    public class OrmTestStartup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public OrmTestStartup(IWebHostEnvironment env, IConfiguration configuration)
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
            builder.RegisterModule(new ConnectorsModule());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => { loggingBuilder.AddSeq(); });
            services.AddHttpContextAccessor();
            
            AuthenticationConfiguration.AddAuthenticationSchemes(services, configuration);
            
            CorsConfiguration.AddCors(services, env);
            services.AddControllers();
            
            Configurations.ConfigureStripe(configuration);
            
            services.ConfigureRealPostgresTestDatabases();
            
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
            var logger = loggerFactory.CreateLogger<InMemoryTestStartup>();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<SetHeadersMiddleware>(); // MUST come after UseAuthentication to ensure we are setting these headers on authenticated requests
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHangfireDashboard();
                    endpoints.MapHealthChecks("/healthcheck");
                });
            hangFireJobs.AddHangFireJobs(app);
        }
    }
}