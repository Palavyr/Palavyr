using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class ToggleShowSeenEnquiriesHandler : IRequestHandler<ToggleShowSeenEnquiriesRequest, ToggleShowSeenEnquiriesResponse>
    {
        private readonly IAccountRepository accountRepository;

        public ToggleShowSeenEnquiriesHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<ToggleShowSeenEnquiriesResponse> Handle(ToggleShowSeenEnquiriesRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();

            var newValue = !account.ShowSeenEnquiries;
            account.ShowSeenEnquiries = newValue;
            await accountRepository.CommitChangesAsync();
            return new ToggleShowSeenEnquiriesResponse(newValue);
        }
    }

    public class ToggleShowSeenEnquiriesResponse
    {
        public ToggleShowSeenEnquiriesResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ToggleShowSeenEnquiriesRequest : IRequest<ToggleShowSeenEnquiriesResponse>
    {
    }
}