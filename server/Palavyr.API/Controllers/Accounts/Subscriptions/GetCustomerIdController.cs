using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    [Route("api")]
    [ApiController]
    public class GetCustomerIdController
    {
        private readonly ILogger<GetCustomerIdController> logger;
        private readonly AccountsContext accountsContext;

        public GetCustomerIdController(
            ILogger<GetCustomerIdController> logger,
            AccountsContext accountsContext
        )
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }

        [HttpGet("payments/customer-id")]
        public async Task<string> Get([FromHeader] string accountId)
        {
            var account = await accountsContext.Accounts.SingleAsync(row => row.AccountId == accountId);
            return account.StripeCustomerId;
        }
    }
}