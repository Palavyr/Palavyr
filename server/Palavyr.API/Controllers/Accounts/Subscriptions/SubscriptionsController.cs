using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Palavyr.API.controllers.accounts.newAccount
{
    [Route("api")]
    [ApiController]
    public class GetNumberOfAreasAllowedBySubscriptionController : ControllerBase
    {
        private readonly AccountsContext accountsContext;

        public GetNumberOfAreasAllowedBySubscriptionController(AccountsContext accountsContext)
        {
            this.accountsContext = accountsContext;
        }

        [HttpGet("subscriptions/count")]
        public async Task<IActionResult> Get([FromHeader] string accountId)
        {
            var numAreasAllowed = (await accountsContext
                .Subscriptions
                .SingleOrDefaultAsync(row => row.AccountId == accountId))
                .NumAreas;
            return Ok(numAreasAllowed);
        }
    }
}