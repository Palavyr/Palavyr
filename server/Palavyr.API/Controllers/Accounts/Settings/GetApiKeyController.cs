using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class GetApiKeyController : PalavyrBaseController
    {
        private IAccountRepository accountRepository;
        private ILogger<GetApiKeyController> logger;

        public const string Uri = "account/settings/api-key";

        public GetApiKeyController(IAccountRepository accountRepository, ILogger<GetApiKeyController> logger)
        {
            this.logger = logger;
            this.accountRepository = accountRepository;
            
        }

        [HttpGet(Uri)]
        public async Task<string> Get()
        {
            var account = await accountRepository.GetAccount();
            return account?.ApiKey ?? "No Api Key Found";
        }
    }
}