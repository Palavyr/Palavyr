using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Palavyr.Background;

namespace Palavyr.API.Registration.BackgroundJobs
{
    public class HangFireJobs
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<HangFireJobs> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IRecurringJobManager recurringJobManager;

        public HangFireJobs(
            IWebHostEnvironment env,
            ILogger<HangFireJobs> logger,
            IServiceProvider serviceProvider,
            IRecurringJobManager recurringJobManager
        )
        {
            this.env = env;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.recurringJobManager = recurringJobManager;
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
                        () => serviceProvider.GetService<ICreatePalavyrSnapshot>().CreateAndTransferCompleteBackup(),
                        Cron.Daily
                    );
                recurringJobManager
                    .AddOrUpdate(
                        "Keep only the last 50 snapshots",
                        () => serviceProvider.GetService<IRemoveOldS3Archives>().RemoveS3Objects(),
                        Cron.Daily
                    );
                recurringJobManager
                    .AddOrUpdate(
                        "Clean Expired Sessions",
                        () => serviceProvider.GetService<IRemoveStaleSessions>().CleanSessionDB(),
                        Cron.Hourly
                    );
                recurringJobManager
                    .AddOrUpdate(
                        "Validate All Attachment DB Entries",
                        () => serviceProvider.GetService<IValidateAttachments>().ValidateAllAttachments(),
                        Cron.Weekly
                    );
                recurringJobManager
                    .AddOrUpdate(
                        "Validate All Files",
                        () => serviceProvider.GetService<IValidateAttachments>().ValidateAllFiles(),
                        Cron.Weekly
                    );
            }
        }
    }
}