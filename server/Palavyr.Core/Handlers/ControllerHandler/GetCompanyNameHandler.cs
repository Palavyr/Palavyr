using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetCompanyNameHandler : IRequestHandler<GetCompanyNameRequest, GetCompanyNameResponse>
    {
        private readonly IEntityStore<Account> accountStore;

        public GetCompanyNameHandler(
            IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<GetCompanyNameResponse> Handle(GetCompanyNameRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            return new GetCompanyNameResponse(account.CompanyName ?? "");
        }
    }

    public class GetCompanyNameResponse
    {
        public GetCompanyNameResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetCompanyNameRequest : IRequest<GetCompanyNameResponse>
    {
    }
}