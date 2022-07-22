using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Services.AccountServices.PlanTypes;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.AccountServices
{
    public class AccountRegistrationMaker : IAccountRegistrationMaker
    {
        private readonly IEntityStore<Subscription> subscriptionStore;
        private readonly IEntityStore<WidgetPreference> widgetPreferenceStore;
        private readonly IMediator mediator;
        private readonly ILogger<AccountRegistrationMaker> logger;
        private readonly IEmailVerificationService emailVerificationService;
        private readonly IPalavyrAccessChecker accessChecker;

        public AccountRegistrationMaker(
            ILogger<AccountRegistrationMaker> logger,
            IEmailVerificationService emailVerificationService,
            IPalavyrAccessChecker accessChecker,
            IEntityStore<Subscription> subscriptionStore,
            IEntityStore<WidgetPreference> widgetPreferenceStore,
            IMediator mediator
        )
        {
            this.subscriptionStore = subscriptionStore;
            this.widgetPreferenceStore = widgetPreferenceStore;
            this.mediator = mediator;
            this.logger = logger;
            this.emailVerificationService = emailVerificationService;
            this.accessChecker = accessChecker;
        }

        public async Task<bool> TryRegisterAccountAndSendEmailVerificationToken(string accountId, string apiKey, string emailAddress, string introId, CancellationToken cancellationToken)
        {
            accessChecker.CheckAccountAccess(emailAddress);

            await CreateNewSubscription(accountId, apiKey);
            await InitializeWidgetPreferences(accountId);

            await mediator.Send(new CreateIntentRequest { IntentName = "My Example Intent" });

            var result = await emailVerificationService.SendConfirmationTokenEmail(emailAddress, cancellationToken);
            return result;
        }

        private async Task InitializeWidgetPreferences(string accountId)
        {
            await widgetPreferenceStore.Create(WidgetPreference.CreateDefault(accountId));
        }

        private async Task CreateNewSubscription(string accountId, string apiKey)
        {
            var freePlanType = new LytePlanTypeMeta();

            // Add the default subscription (free with 2 intents)
            logger.LogDebug("Add default subscription for {AccountId}", accountId);
            var newSubscription = Subscription.CreateNew(accountId, apiKey, freePlanType.GetDefaultNumIntents());
            await subscriptionStore.Create(newSubscription);
        }
    }
}