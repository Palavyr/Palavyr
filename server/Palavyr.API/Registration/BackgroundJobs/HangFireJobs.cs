using System;
using Autofac;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Palavyr.Core.BackgroundJobs;

namespace Palavyr.API.Registration.BackgroundJobs
{
    public class HangFireJobs
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<HangFireJobs> logger;
        private readonly IRecurringJobManager recurringJobManager;
        private readonly ILifetimeScope container;

        public HangFireJobs(
            IWebHostEnvironment env,
            ILogger<HangFireJobs> logger,
            IRecurringJobManager recurringJobManager,
            ILifetimeScope container
        )
        {
            this.env = env;
            this.logger = logger;
            this.recurringJobManager = recurringJobManager;
            this.container = container;
        }

        public void AddHangFireJobs(IApplicationBuilder app)
        {
            Action staleSessionAction = () => container.Resolve<IRemoveStaleSessions>().CleanSessionDB();
            
            if (env.IsProduction())
            {
                logger.LogDebug($"Current env for hangfire: {env.EnvironmentName}");
                logger.LogDebug("Setting up the hangfire dashboard");
                app.UseHangfireDashboard();

                recurringJobManager
                    .AddOrUpdate(
                        "Clean Expired Sessions",
                        () => staleSessionAction(),
                        Cron.Hourly
                    );
            }
        }
    }
}