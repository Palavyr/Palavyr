using System.Linq;
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
    public class ModifyUserNameController : ControllerBase
    {
        private ILogger<ModifyUserNameController> logger;
        private AccountsContext accountsContext;

        public ModifyUserNameController(AccountsContext accountsContext, ILogger<ModifyUserNameController> logger)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }
        
        [HttpPut("account/settings/username")]
        public async Task<IActionResult> UpdateUserName([FromHeader] string accountId, LoginCredentials login)
        {
            var allUserNames = accountsContext.Accounts.ToList().Select(x => x.UserName);
            if (allUserNames.Contains(login.Username))
            {
                return BadRequest();
            }

            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            account.UserName = login.Username;
            await accountsContext.SaveChangesAsync();
            return NoContent();
        }
    }
}