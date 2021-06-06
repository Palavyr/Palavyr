using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;

namespace Palavyr.API.Controllers.Accounts.Setup
{

    public class GetAccountActiveStatusController : PalavyrBaseController
    {
        private AccountsContext accountsContext;
        private ILogger<GetAccountActiveStatusController> logger;

        public GetAccountActiveStatusController(
            ILogger<GetAccountActiveStatusController> logger,
            AccountsContext accountsContext
        )
        {
            this.accountsContext = accountsContext;
            this.logger = logger;
        }

        [HttpGet("account/is-active")]
        public async Task<bool> CheckIsActive([FromHeader] string accountId)
        {
            logger.LogDebug("Activation controller hit! Again!");
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            var isActive = account.Active;
            return isActive;
        }
    }
}