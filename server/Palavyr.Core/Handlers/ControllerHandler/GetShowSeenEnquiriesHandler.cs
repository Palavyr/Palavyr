using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetShowSeenEnquiriesHandler : IRequestHandler<GetShowSeenEnquiriesRequest, GetShowSeenEnquiriesResponse>
    {
        private readonly IConfigurationEntityStore<Account> accountStore;

        public GetShowSeenEnquiriesHandler(IConfigurationEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<GetShowSeenEnquiriesResponse> Handle(GetShowSeenEnquiriesRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            var show = account.ShowSeenEnquiries;
            return new GetShowSeenEnquiriesResponse(show);
        }
    }

    public class GetShowSeenEnquiriesResponse
    {
        public GetShowSeenEnquiriesResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class GetShowSeenEnquiriesRequest : IRequest<GetShowSeenEnquiriesResponse>
    {
    }
}