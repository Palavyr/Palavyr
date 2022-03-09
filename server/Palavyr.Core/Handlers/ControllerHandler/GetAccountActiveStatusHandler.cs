using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAccountActiveStatusHandler : IRequestHandler<GetAccountActiveStatusRequest, GetAccountActiveStatusResponse>
    {
        private readonly IConfigurationEntityStore<Account> accountStore;
        private readonly ILogger<GetAccountActiveStatusHandler> logger;

        public GetAccountActiveStatusHandler(IConfigurationEntityStore<Account> accountStore, ILogger<GetAccountActiveStatusHandler> logger)
        {
            this.accountStore = accountStore;
            this.logger = logger;
        }

        public async Task<GetAccountActiveStatusResponse> Handle(GetAccountActiveStatusRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Activation controller hit! Again!");
            var account = await accountStore.GetAccount();
            var isActive = account.Active;
            return new GetAccountActiveStatusResponse(isActive);
        }
    }

    public class GetAccountActiveStatusResponse
    {
        public GetAccountActiveStatusResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class GetAccountActiveStatusRequest : IRequest<GetAccountActiveStatusResponse>
    {
    }
}