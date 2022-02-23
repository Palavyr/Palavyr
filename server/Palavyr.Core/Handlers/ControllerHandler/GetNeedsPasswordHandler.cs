using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetNeedsPasswordHandler : IRequestHandler<GetNeedsPasswordRequest, GetNeedsPasswordResponse>
    {
        private readonly IAccountRepository accountRepository;
        private static readonly int[] NeedsPassword = new[] { 0 };

        public GetNeedsPasswordHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<GetNeedsPasswordResponse> Handle(GetNeedsPasswordRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            return new GetNeedsPasswordResponse(NeedsPassword.Contains((int)(account.AccountType)));
        }
    }

    public class GetNeedsPasswordResponse
    {
        public GetNeedsPasswordResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class GetNeedsPasswordRequest : IRequest<GetNeedsPasswordResponse>
    {
    }
}