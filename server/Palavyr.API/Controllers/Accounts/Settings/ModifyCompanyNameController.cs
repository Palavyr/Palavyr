using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    [Route("api")]
    [ApiController]
    public class ModifyCompanyNameController : ControllerBase
    {
        private ILogger<ModifyCompanyNameController> logger;
        private AccountsContext accountsContext;

        public ModifyCompanyNameController(AccountsContext accountsContext, ILogger<ModifyCompanyNameController> logger)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }
        
        [HttpPut("account/settings/company-name")]
        public async Task<IActionResult> UpdateCompanyName([FromHeader] string accountId, LoginCredentials login)
        {
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.AccountId == accountId);
            account.CompanyName = login.CompanyName;
            await accountsContext.SaveChangesAsync();
            return NoContent();
        }
    }
}