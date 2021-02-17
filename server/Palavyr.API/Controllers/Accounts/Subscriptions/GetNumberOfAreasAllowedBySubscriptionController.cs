using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
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
        public async Task<int> Get([FromHeader] string accountId)
        {
            var numAreasAllowed = (await accountsContext
                .Subscriptions
                .SingleAsync(row => row.AccountId == accountId))
                .NumAreas;
            return numAreasAllowed;
        }
    }
}