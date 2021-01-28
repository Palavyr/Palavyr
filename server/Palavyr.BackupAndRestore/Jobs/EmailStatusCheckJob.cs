using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using EmailService.Verification;
using Microsoft.Extensions.Logging;

namespace Palavyr.BackupAndRestore.Jobs
{
    public class EmailStatusCheckJob
    {
        private readonly ILogger<EmailStatusCheckJob> logger;
        private readonly AccountsContext accountsContext;
        private readonly DashContext dashContext;
        private readonly EmailVerificationStatus emailVerificationStatus;

        public EmailStatusCheckJob(
            ILogger<EmailStatusCheckJob> logger,
            AccountsContext accountsContext,
            DashContext dashContext,
            EmailVerificationStatus emailVerificationStatus
        )
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.dashContext = dashContext;
            this.emailVerificationStatus = emailVerificationStatus;
        }

        public async Task CheckAndUpdate()
        {
            logger.LogDebug("Checking Account emails...");
            Console.WriteLine("Checking Account emails...");
            var accounts = accountsContext.Accounts;
            foreach (var account in accounts)
            {
                var emailToCheck = account.EmailAddress;
                bool isVerified;
                try
                {
                    isVerified = await emailVerificationStatus.CheckVerificationStatus(emailToCheck);
                    account.DefaultEmailIsVerified = isVerified;
                    logger.LogDebug($"{emailToCheck} is verified: {isVerified}");
                    Console.WriteLine($"{emailToCheck} is verified: {isVerified}");
                }
                catch (Exception ex)
                {
                    var message = "Account found without a default email. THIS SHOULD NOT BE. Investigate immediately.";
                    Console.WriteLine(message);
                    logger.LogDebug(message);
                }
            }

            await accountsContext.SaveChangesAsync();

            logger.LogDebug("Checking area emails...");
            Console.WriteLine("Checking area emails...");
            var areas = dashContext.Areas;
            foreach (var area in areas)
            {
                var emailToCheck = area.AreaSpecificEmail;
                if (!string.IsNullOrWhiteSpace(emailToCheck))
                {
                    var isVerified = await emailVerificationStatus.CheckVerificationStatus(emailToCheck);
                    area.EmailIsVerified = isVerified;
                    logger.LogDebug($"Area: {area.AreaName} -- {emailToCheck} is verified: {isVerified}");
                    Console.WriteLine($"{emailToCheck} is verified: {isVerified}");
                }
                else
                {
                    logger.LogDebug($"Area {area.AreaName} doesn't not specify an email address.");
                    Console.WriteLine($"Area {area.AreaName} doesn't not specify an email address.");
                }
            }

            await dashContext.SaveChangesAsync();
        }
    }
}