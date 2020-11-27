using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes.Registration;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    [Route("api")]
    [ApiController]
    public class ModifyLocaleController : ControllerBase
    {
        private ILogger<ModifyLocaleController> logger;
        private AccountsContext accountsContext;

        public ModifyLocaleController(AccountsContext accountsContext, ILogger<ModifyLocaleController> logger)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }

        [HttpPut("account/settings/locale")]
        public async Task<string> Modify([FromHeader] string accountId, AccountDetails accountDetails)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            account.Locale = accountDetails.Locale;
            await accountsContext.SaveChangesAsync();
            return account.Locale;
        }
    }
}