using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class GetApiKeyHandler : IRequestHandler<GetApiKeyRequest, GetApiKeyResponse>
    {
        private readonly IAccountRepository accountRepository;

        public GetApiKeyHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<GetApiKeyResponse> Handle(GetApiKeyRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            if (account.ApiKey == null) throw new DomainException("No Api Key Found For this account");
            return new GetApiKeyResponse(account.ApiKey);
        }
    }

    public class GetApiKeyResponse
    {
        public GetApiKeyResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetApiKeyRequest : IRequest<GetApiKeyResponse>
    {
    }
}