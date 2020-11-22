using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.AccountDB;

namespace Palavyr.API.controllers.accounts.newAccount
{
    [Route("api")]
    [ApiController]
    public class ModifyPasswordController : ControllerBase
    {
        private ILogger<ModifyPasswordController> logger;
        private AccountsContext accountsContext;

        public ModifyPasswordController(AccountsContext accountsContext, ILogger<ModifyPasswordController> logger)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }
        
        [HttpPut("account/settings/update/password")]
        public async Task<IActionResult> UpdatePassword([FromHeader] string accountId, AccountDetails accountDetails)
        {
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.AccountId == accountId);

            var oldHashedPassword = accountDetails.OldPassword;
            if (oldHashedPassword != accountDetails.Password)
            {
                return Ok(false);
            }

            account.Password = accountDetails.Password;
            await accountsContext.SaveChangesAsync();
            return Ok(true);
        }
    }
}