using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    public class GetCustomerIdController : PalavyrBaseController
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