using System;
using Autofac;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Palavyr.BackupAndRestore;
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
            if (env.IsProduction())
            {
                logger.LogDebug("Current env for hangfire: {env");
                logger.LogDebug("Setting up the hangfire dashboard");
                app.UseHangfireDashboard();

                recurringJobManager
                    .AddOrUpdate(
                        "Backup database",
                        () => container.Resolve<ICreatePalavyrSnapshot>().CreateAndTransferCompleteBackup(),
                        Cron.Daily
                    );
                recurringJobManager
                    .AddOrUpdate(
                        "Keep only the last 50 snapshots",
                        () => container.Resolve<IRemoveOldS3Archives>().RemoveS3Objects(),
                        Cron.Daily
                    );
                recurringJobManager
                    .AddOrUpdate(
                        "Clean Expired Sessions",
                        () => container.Resolve<IRemoveStaleSessions>().CleanSessionDB(),
                        Cron.Hourly
                    );
                recurringJobManager
                    .AddOrUpdate(
                        "Validate All Attachment DB Entries",
                        () => container.Resolve<IValidateAttachments>().ValidateAllAttachments(),
                        Cron.Weekly
                    );
                recurringJobManager
                    .AddOrUpdate(
                        "Validate All Files",
                        () => container.Resolve<IValidateAttachments>().ValidateAllFiles(),
                        Cron.Weekly
                    );
            }
        }
    }
}