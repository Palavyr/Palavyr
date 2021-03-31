using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class GetCompanyNameController : PalavyrBaseController
    {
        private AccountsContext accountsContext;
        private ILogger<GetCompanyNameController> logger;
        public GetCompanyNameController(AccountsContext accountsContext, ILogger<GetCompanyNameController> logger)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }
        
        [HttpGet("account/settings/company-name")]
        public async Task<string> Get([FromHeader] string accountId)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            return account.CompanyName ?? "";
        }
    }
}