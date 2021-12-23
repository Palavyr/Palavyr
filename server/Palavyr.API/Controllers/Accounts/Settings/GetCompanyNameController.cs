using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class GetCompanyNameController : PalavyrBaseController
    {
        private readonly IAccountRepository accountRepository;
        private ILogger<GetCompanyNameController> logger;
        public GetCompanyNameController(IAccountRepository accountRepository, ILogger<GetCompanyNameController> logger)
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
        }
        
        [HttpGet("account/settings/company-name")]
        public async Task<string> Get()
        {
            var account = await accountRepository.GetAccount();
            return account.CompanyName ?? "";
        }
    }
}