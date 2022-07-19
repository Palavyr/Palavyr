using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Data.Setup.SeedData;
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
        private readonly IEntityStore<Intent> intentStore;
        private readonly IEntityStore<WidgetPreference> widgetPreferenceStore;
        private readonly IEntityStore<CategorySelectTableRow> defaultPricingStrategyStore;
        private readonly IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore;
        private readonly IEntityStore<ConversationNode> convoNodeStore;

        public AccountRegistrationMaker(
            ILogger<AccountRegistrationMaker> logger,
            IEmailVerificationService emailVerificationService,
            IPalavyrAccessChecker accessChecker,
            IEntityStore<Intent> intentStore,
            IEntityStore<Subscription> subscriptionStore,
            IEntityStore<WidgetPreference> widgetPreferenceStore,
            IEntityStore<CategorySelectTableRow> defaultPricingStrategyStore,
            IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore,
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
            this.pricingStrategyTableMetaStore = pricingStrategyTableMetaStore;
            this.convoNodeStore = convoNodeStore;
        }

        private async Task InstallSeedData(string accountId, string emailAddress, string introId)
        {
            logger.LogDebug("Install new account seed data.");
            var seedData = new SeedData(accountId, emailAddress, introId);
            await intentStore.CreateMany(seedData.Intents);
            await widgetPreferenceStore.Create(seedData.WidgetPreference);
            await defaultPricingStrategyStore.CreateMany(seedData.DefaultPricingStrategyTables);
            await convoNodeStore.CreateMany(seedData.IntroductionConversationNodes);
            
            seedData.DefaultPricingStrategyTableMetas[0].Id = null; // no idea why I have to do this.
            await pricingStrategyTableMetaStore.CreateMany(seedData.DefaultPricingStrategyTableMetas);
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

            // Add the default subscription (free with 2 intents)
            logger.LogDebug($"Add default subscription for {accountId}");
            var newSubscription = Subscription.CreateNew(accountId, apiKey, freePlanType.GetDefaultNumIntents());
            await subscriptionStore.Create(newSubscription);
        }
    }
}