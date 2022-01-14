using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class ModifyCompanyNameHandler : IRequestHandler<ModifyCompanyNameRequest, ModifyCompanyNameResponse>
    {
        private readonly IAccountRepository accountRepository;

        public ModifyCompanyNameHandler(IAccountRepository  accountRepository, ILogger<ModifyCompanyNameHandler> logger)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<ModifyCompanyNameResponse> Handle(ModifyCompanyNameRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            account.CompanyName = request.CompanyName;
            await accountRepository.CommitChangesAsync();
            return new ModifyCompanyNameResponse(account.CompanyName);
        }
    }

    public class ModifyCompanyNameResponse
    {
        public ModifyCompanyNameResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyCompanyNameRequest : IRequest<ModifyCompanyNameResponse>
    {
        public string CompanyName { get; set; }
    }
}
