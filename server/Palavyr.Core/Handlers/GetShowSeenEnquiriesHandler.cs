using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class GetShowSeenEnquiriesHandler : IRequestHandler<GetShowSeenEnquiriesRequest, GetShowSeenEnquiriesResponse>
    {
        private readonly IAccountRepository accountRepository;

        public GetShowSeenEnquiriesHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<GetShowSeenEnquiriesResponse> Handle(GetShowSeenEnquiriesRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
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