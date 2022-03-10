using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetStripeCustomerIdHandler : IRequestHandler<GetStripeCustomerIdRequest, GetStripeCustomerIdResponse>
    {
        private readonly IEntityStore<Account> accountStore;

        public GetStripeCustomerIdHandler(
            ILogger<GetStripeCustomerIdHandler> logger,
            IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<GetStripeCustomerIdResponse> Handle(GetStripeCustomerIdRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            return new GetStripeCustomerIdResponse(account.StripeCustomerId);
        }
    }

    public class GetStripeCustomerIdResponse
    {
        public GetStripeCustomerIdResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetStripeCustomerIdRequest : IRequest<GetStripeCustomerIdResponse>
    {
    }
}