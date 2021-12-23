using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    public class GetCustomerIdController : PalavyrBaseController
    {
        private readonly ILogger<GetCustomerIdController> logger;
        private readonly IAccountRepository accountRepository;

        public GetCustomerIdController(
            ILogger<GetCustomerIdController> logger,
            IAccountRepository accountRepository
        )
        {
            this.logger = logger;
            this.accountRepository = accountRepository;
        }

        [HttpGet("payments/customer-id")]
        public async Task<string> Get()
        {
            var account = await accountRepository.GetAccount();
            return account.StripeCustomerId;
        }
    }
}