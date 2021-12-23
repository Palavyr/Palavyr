using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class GetApiKeyController : PalavyrBaseController
    {
        private AccountsContext accountsContext;
        private readonly IHoldAnAccountId accountIdHolder;
        private ILogger<GetApiKeyController> logger;

        public const string Uri = "account/settings/api-key";

        public GetApiKeyController(IHoldAnAccountId accountIdHolder, AccountsContext accountsContext, ILogger<GetApiKeyController> logger)
        {
            this.accountIdHolder = accountIdHolder;
            this.logger = logger;
            this.accountsContext = accountsContext;
            
        }

        [HttpGet(Uri)]
        public async Task<string> Get()
        {
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(
                    row => row.AccountId == accountIdHolder.AccountId);
            return account?.ApiKey ?? "No Api Key Found";
        }
    }
}