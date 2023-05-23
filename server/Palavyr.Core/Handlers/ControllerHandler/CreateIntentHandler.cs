using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateIntentHandler : IRequestHandler<CreateIntentRequest, CreateIntentResponse>
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly ILogger<CreateIntentHandler> logger;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly IEntityStore<Intent> intentStore;

        public CreateIntentHandler(
            IEntityStore<Account> accountStore,
            ILogger<CreateIntentHandler> logger,
            IAccountIdTransport accountIdTransport,
            IEntityStore<Intent> intentStore)
        {
            this.accountStore = accountStore;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
            this.intentStore = intentStore;
        }

        public async Task<CreateIntentResponse> Handle(CreateIntentRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.Get(accountStore.AccountId, s => s.AccountId);

            var defaultEmail = account.EmailAddress;
            var isVerified = account.DefaultEmailIsVerified;

            logger.LogInformation("Creating new intent for account: {Account} called {IntentName}", accountIdTransport.AccountId, request.IntentName);
            var newIntent = Intent.CreateNewIntent(request.IntentName, accountIdTransport.AccountId, defaultEmail, isVerified);
            if (request.IntentId is not null)
            {
                newIntent.UpdateIntentId(request.IntentId);
            }

            await intentStore.Create(newIntent);
            return new CreateIntentResponse(newIntent.IntentId);
        }
    }

    public class CreateIntentRequest : IRequest<CreateIntentResponse>
    {
        public const string Route = "intents/create";
        public string IntentName { get; set; }
        public string? IntentId { get; set; }
    }

    public class CreateIntentResponse
    {
        public string IntentId { get; }

        public CreateIntentResponse(string intentId)
        {
            IntentId = intentId;
        }
    }
}