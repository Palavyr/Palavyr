using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
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
        private readonly IEntityStore<Area> intentStore;
        private readonly IMapToNew<Area, IntentResource> mapper;

        public CreateIntentHandler(
            IEntityStore<Account> accountStore,
            ILogger<CreateIntentHandler> logger,
            IAccountIdTransport accountIdTransport,
            IEntityStore<Area> intentStore,
            IMapToNew<Area, IntentResource> mapper)
        {
            this.accountStore = accountStore;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
            this.intentStore = intentStore;
            this.mapper = mapper;
        }

        public async Task<CreateIntentResponse> Handle(CreateIntentRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.Get(accountStore.AccountId, s => s.AccountId);

            var defaultEmail = account.EmailAddress;
            var isVerified = account.DefaultEmailIsVerified;

            logger.LogInformation($"Creating new area for account: {accountIdTransport.AccountId} called {request.AreaName}");
            var newIntent = Area.CreateNewIntent(request.AreaName, accountIdTransport.AccountId, defaultEmail, isVerified);
            var intent = await intentStore.Create(newIntent);

            var resource = await mapper.Map(intent);
            return new CreateIntentResponse(resource);
        }
    }

    public class CreateIntentRequest : IRequest<CreateIntentResponse>
    {
        public const string Route = "intents/create";

        public string AreaName { get; set; }
    }

    public class UpdateIntentNameRequest : CreateIntentRequest
    {
    }

    public class CreateIntentResponse
    {
        public readonly IntentResource Response;

        public CreateIntentResponse(IntentResource response) => Response = response;
    }
}