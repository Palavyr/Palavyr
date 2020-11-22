using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.controllers.accounts.newAccount
{
    [Route("api")]
    [ApiController]
    public class GetLocaleController : ControllerBase
    {
        private AccountsContext accountsContext;
        private ILogger<GetLocaleController> logger;
        public GetLocaleController(AccountsContext accountsContext, ILogger<GetLocaleController> logger)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }
        
        [HttpGet("account/settings/locale")]
        public async Task<IActionResult> Get([FromHeader] string accountId)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            return Ok(account.Locale ?? "");
        }
        
    }
}