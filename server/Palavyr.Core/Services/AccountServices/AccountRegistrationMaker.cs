using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Setup.SeedData;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Services.AccountServices.PlanTypes;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.AccountServices
{
    public class AccountRegistrationMaker : IAccountRegistrationMaker
    {
        private readonly IEntityStore<Subscription> subscriptionStore;
        private readonly ILogger<AccountRegistrationMaker> logger;
        private readonly IEmailVerificationService emailVerificationService;
        private readonly IPalavyrAccessChecker accessChecker;
        private readonly IEntityStore<Area> intentStore;
        private readonly IEntityStore<WidgetPreference> widgetPreferenceStore;
        private readonly IEntityStore<SelectOneFlat> defaultPricingStrategyStore;
        private readonly IEntityStore<DynamicTableMeta> dynamicTableMetaStore;
        private readonly IEntityStore<ConversationNode> convoNodeStore;

        public AccountRegistrationMaker(
            ILogger<AccountRegistrationMaker> logger,
            IEmailVerificationService emailVerificationService,
            IPalavyrAccessChecker accessChecker,
            IEntityStore<Area> intentStore,
            IEntityStore<Subscription> subscriptionStore,
            IEntityStore<WidgetPreference> widgetPreferenceStore,
            IEntityStore<SelectOneFlat> defaultPricingStrategyStore,
            IEntityStore<DynamicTableMeta> dynamicTableMetaStore,
            IEntityStore<ConversationNode> convoNodeStore
        )
        {
            this.subscriptionStore = subscriptionStore;
            this.logger = logger;
            this.emailVerificationService = emailVerificationService;
            this.accessChecker = accessChecker;
            this.intentStore = intentStore;
            this.widgetPreferenceStore = widgetPreferenceStore;
            this.defaultPricingStrategyStore = defaultPricingStrategyStore;
            this.dynamicTableMetaStore = dynamicTableMetaStore;
            this.convoNodeStore = convoNodeStore;
        }

        private async Task InstallSeedData(string accountId, string emailAddress, string introId)
        {
            logger.LogDebug("Install new account seed data.");
            var seedData = new SeedData(accountId, emailAddress, introId);
            await intentStore.CreateMany(seedData.Areas);
            await widgetPreferenceStore.Create(seedData.WidgetPreference);
            await defaultPricingStrategyStore.CreateMany(seedData.DefaultDynamicTables);
            await convoNodeStore.CreateMany(seedData.IntroductionConversationNodes);
            
            seedData.DefaultDynamicTableMetas[0].Id = null; // no idea why I have to do this.
            await dynamicTableMetaStore.CreateMany(seedData.DefaultDynamicTableMetas);
        }

        public async Task<bool> TryRegisterAccountAndSendEmailVerificationToken(string accountId, string apiKey, string emailAddress, string introId, CancellationToken cancellationToken)
        {
            accessChecker.CheckAccountAccess(emailAddress);

            await CreateNewSubscription(accountId, apiKey);
            await InstallSeedData(accountId, emailAddress, introId);

            var result = await emailVerificationService.SendConfirmationTokenEmail(emailAddress, cancellationToken);
            return result;
        }

        private async Task CreateNewSubscription(string accountId, string apiKey)
        {
            var freePlanType = new LytePlanTypeMeta();

            // Add the default subscription (free with 2 areas)
            logger.LogDebug($"Add default subscription for {accountId}");
            var newSubscription = Subscription.CreateNew(accountId, apiKey, freePlanType.GetDefaultNumAreas());
            await subscriptionStore.Create(newSubscription);
        }
    }
}