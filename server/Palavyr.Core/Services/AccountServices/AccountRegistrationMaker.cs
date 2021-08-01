using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Data.Setup.SeedData;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.AccountServices.PlanTypes;

namespace Palavyr.Core.Services.AccountServices
{
    public class AccountRegistrationMaker : IAccountRegistrationMaker
    {
        private readonly ILogger<AccountRegistrationMaker> logger;
        private readonly AccountsContext accountsContext;
        private readonly DashContext dashContext;
        private readonly IEmailVerificationService emailVerificationService;
        private readonly IPalavyrAccessChecker accessChecker;

        public AccountRegistrationMaker(
            ILogger<AccountRegistrationMaker> logger,
            AccountsContext accountsContext,
            DashContext dashContext,
            IEmailVerificationService emailVerificationService,
            IPalavyrAccessChecker accessChecker
        )
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.dashContext = dashContext;
            this.emailVerificationService = emailVerificationService;
            this.accessChecker = accessChecker;
        }


        public async Task<bool> TryRegisterAccountAndSendEmailVerificationToken(string accountId, string apiKey, string emailAddress, CancellationToken cancellationToken)
        {
            accessChecker.CheckAccountAccess(emailAddress);

            await CreateNewSubscription(accountId, apiKey);
            await InstallSeedData(accountId, emailAddress);

            var result = await emailVerificationService.SendConfirmationTokenEmail(emailAddress, accountId, cancellationToken);
            return result;
        }

        private async Task CreateNewSubscription(string accountId, string apiKey)
        {
            var freePlanType = new LytePlanTypeMeta();

            // Add the default subscription (free with 2 areas)
            logger.LogDebug($"Add default subscription for {accountId}");
            var newSubscription = Subscription.CreateNew(accountId, apiKey, freePlanType.GetDefaultNumAreas());
            await accountsContext.Subscriptions.AddAsync(newSubscription);
        }

        private async Task InstallSeedData(string accountId, string emailAddress)
        {
            logger.LogDebug("Install new account seed data.");
            var seedData = new SeedData(accountId, emailAddress);
            await dashContext.Areas.AddRangeAsync(seedData.Areas);
            await dashContext.WidgetPreferences.AddAsync(seedData.WidgetPreference);
            await dashContext.SelectOneFlats.AddRangeAsync(seedData.DefaultDynamicTables);
            await dashContext.DynamicTableMetas.AddRangeAsync(seedData.DefaultDynamicTableMetas);
        }
    }
}