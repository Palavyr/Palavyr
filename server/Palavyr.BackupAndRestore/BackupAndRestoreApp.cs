using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using EmailService.Verification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.BackupAndRestore.UserData;
using Palavyr.Common.UIDUtils;

namespace Palavyr.BackupAndRestore
{
    public class BackupAndRestoreApp
    {
        private readonly ILogger<BackupAndRestoreApp> logger;
        private readonly EmailVerificationStatus emailVerificationStatus;
        private readonly DashContext dashContext;
        private readonly AccountsContext accountsContext;
        private readonly Operations operations;
        private readonly PostgresRestorer postgresRestorer;
        private readonly IConfiguration configuration;
        private readonly IPostgresBackup postgresBackup;
        private readonly IUserDataBackup userDataBackup;
        private readonly UpdateDatabaseLatest updateDatabaseLatest;

        public BackupAndRestoreApp(
            ILogger<BackupAndRestoreApp> logger,
            EmailVerificationStatus emailVerificationStatus,
            DashContext dashContext,
            AccountsContext accountsContext,
            Operations operations, 
            PostgresRestorer postgresRestorer, 
            IConfiguration configuration,
            IPostgresBackup postgresBackup,
            IUserDataBackup userDataBackup,
            UpdateDatabaseLatest updateDatabaseLatest
            )
        {
            this.logger = logger;
            this.emailVerificationStatus = emailVerificationStatus;
            this.dashContext = dashContext;
            this.accountsContext = accountsContext;
            this.operations = operations;
            this.postgresRestorer = postgresRestorer;
            this.configuration = configuration;
            this.postgresBackup = postgresBackup;
            this.userDataBackup = userDataBackup;
            this.updateDatabaseLatest = updateDatabaseLatest;
        }

        public async Task Execute(string[] args)
        {
            var bucket = configuration.GetSection(DatabaseConstants.BackupsSection).Value;
            var host = configuration.GetSection(DatabaseConstants.PostgresHost).Value;
            var port = configuration.GetSection(DatabaseConstants.PostgresPort).Value;
            var pass = configuration.GetSection(DatabaseConstants.PostgresPassword).Value;

            if (args.Length == 0 || args.Length > 1)
            {
                Console.WriteLine();
                Console.WriteLine("Please provide only one of the following arguments: --check, --restore, --backup");
            }
            
            if (args[0] == "--check")
            {
                await operations.RestoreAndCheckAndCleanup(bucket, host, port, pass);
            }
            else if (args[0] == "--restore")
            {
                await postgresRestorer.PerformStandardRestore( host, port, pass);
                Console.WriteLine("Completed restoring your database!");
            }
            else if (args[0] == "--backup")
            {
                
                var timeStamp = TimeUtils.CreateTimeStamp();
                var latestDatabaseBackup = await postgresBackup.CreateFullDatabaseBackup(host, port, pass, timeStamp, bucket);
                var latestUserDataBackup = await userDataBackup.CreateFullUserDataBackup(timeStamp, bucket);
                await updateDatabaseLatest.UpdateLatestBackupRecords(latestDatabaseBackup, latestUserDataBackup);
                Console.WriteLine();
                Console.WriteLine("Completed creating a new backup!");
            }
            else if (args[0] == "--check-email-status")
            {
                
                logger.LogDebug("Checking Account emails...");
                Console.WriteLine("Checking Account emails...");
                var accounts = accountsContext.Accounts;
                foreach (var account in accounts)
                {
                    var emailToCheck = account.EmailAddress;
                    var isVerified = await emailVerificationStatus.CheckVerificationStatus(emailToCheck);
                    account.DefaultEmailIsVerified = isVerified;
                    logger.LogDebug($"{emailToCheck} is verified: {isVerified}");
                    Console.WriteLine($"{emailToCheck} is verified: {isVerified}");
                }

                await accountsContext.SaveChangesAsync();

                logger.LogDebug("Checking area emails...");
                Console.WriteLine("Checking area emails...");
                var areas = dashContext.Areas;
                foreach (var area in areas)
                {
                    var emailToCheck = area.AreaSpecificEmail;
                    var isVerified = await emailVerificationStatus.CheckVerificationStatus(emailToCheck);
                    area.EmailIsVerified = isVerified;
                    logger.LogDebug($"Area: {area.AreaName} -- {emailToCheck} is verified: {isVerified}");
                    Console.WriteLine($"{emailToCheck} is verified: {isVerified}");
                }
                await dashContext.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Please provide only one of the following arguments: --check, --restore, --backup, --check-email-status");
            }
        }
    }
}