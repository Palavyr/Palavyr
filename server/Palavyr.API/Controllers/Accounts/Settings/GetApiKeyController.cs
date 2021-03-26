using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class GetApiKeyController : PalavyrBaseController
    {
        private AccountsContext accountsContext;
        private ILogger<GetApiKeyController> logger;

        public GetApiKeyController(AccountsContext accountsContext, ILogger<GetApiKeyController> logger)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }

        [HttpGet("account/settings/api-key")]
        public async Task<string> Get([FromHeader] string accountId)
        {
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(
                    row => row.AccountId == accountId);
            return account.ApiKey ?? "No Api Key Found";
        }
    }
}