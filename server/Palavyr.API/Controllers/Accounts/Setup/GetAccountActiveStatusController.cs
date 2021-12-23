using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Accounts.Setup
{

    public class GetAccountActiveStatusController : PalavyrBaseController
    {
        private ILogger<GetAccountActiveStatusController> logger;
        private IAccountRepository accountRepository;

        public GetAccountActiveStatusController(
            ILogger<GetAccountActiveStatusController> logger,
            IAccountRepository accountRepository
        )
        {
            this.logger = logger;
            this.accountRepository = accountRepository;
        }

        [HttpGet("account/is-active")]
        public async Task<bool> CheckIsActive()
        {
            logger.LogDebug("Activation controller hit! Again!");
            var account = await accountRepository.GetAccount();
            var isActive = account.Active;
            return isActive;
        }
    }
}