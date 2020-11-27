using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    [Route("api")]
    [ApiController]
    public class GetUserNameController : ControllerBase
    {
        private AccountsContext accountsContext;
        private ILogger<GetUserNameController> logger;
        public GetUserNameController(AccountsContext accountsContext, ILogger<GetUserNameController> logger)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }
        
        [HttpGet("account/settings/user-name")]
        public async Task<IActionResult> Get([FromHeader] string accountId)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            return Ok(account.UserName ?? "");
        }

        
    }
}