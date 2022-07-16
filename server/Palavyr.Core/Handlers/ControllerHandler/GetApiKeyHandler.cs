using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetApiKeyHandler : IRequestHandler<GetApiKeyRequest, GetApiKeyResponse>
    {
        private readonly IEntityStore<Account> accountStore;

        public GetApiKeyHandler(IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<GetApiKeyResponse> Handle(GetApiKeyRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            if (string.IsNullOrEmpty(account.ApiKey) || string.IsNullOrWhiteSpace(account.ApiKey)) throw new DomainException("No Api Key Found For this account");
            return new GetApiKeyResponse(account.ApiKey);
        }
    }

    public class GetApiKeyRequest : IRequest<GetApiKeyResponse>
    {
        public const string Route = "account/settings/api-key";
    }

    public class GetApiKeyResponse
    {
        public GetApiKeyResponse(string response) => Response = response;
        public string Response { get; set; }
    }
}