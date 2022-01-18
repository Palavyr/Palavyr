using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class GetCompanyNameHandler : IRequestHandler<GetCompanyNameRequest, GetCompanyNameResponse>
    {
        private readonly IAccountRepository accountRepository;

        public GetCompanyNameHandler(
            IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<GetCompanyNameResponse> Handle(GetCompanyNameRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
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