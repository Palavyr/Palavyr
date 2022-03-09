using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateIntentHandler : IRequestHandler<CreateIntentRequest, CreateIntentResponse>
    {
        private readonly IConfigurationEntityStore<Account> accountStore;
        private readonly ILogger<CreateIntentHandler> logger;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly IConfigurationEntityStore<Area> intentStore;

        public CreateIntentHandler(
            IConfigurationEntityStore<Account> accountStore,
            ILogger<CreateIntentHandler> logger,
            IAccountIdTransport accountIdTransport,
            IConfigurationEntityStore<Area> intentStore)
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

            logger.LogInformation($"Creating new area for account: {accountIdTransport.AccountId} called {request.AreaName}");
            var newIntent = Area.CreateNewArea(request.AreaName, accountIdTransport.AccountId, defaultEmail, isVerified);
            var intent = await intentStore.Create(newIntent);

            return new CreateIntentResponse(intent);
        }
    }
    
    public class CreateIntentRequest : IRequest<CreateIntentResponse>
    {
        public string AreaName { get; set; }
    }
    
    public class UpdateIntentNameRequest : CreateIntentRequest {} 

    public class CreateIntentResponse
    {
        public readonly Area Response;

        public CreateIntentResponse(Area response) => Response = response;
    }
}