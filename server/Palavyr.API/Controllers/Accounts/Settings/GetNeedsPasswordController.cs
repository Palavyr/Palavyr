using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetNeedsPasswordController : PalavyrBaseController
    {
        private readonly IAccountRepository accountRepository;
        private static readonly int[] NeedsPassword = new []{ 0 };

        public GetNeedsPasswordController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        
        [HttpGet("account/needs-password")]
        public async Task<bool> Get([FromHeader] string accountId)
        {
            var account = await accountRepository.GetAccount(accountId);
            return NeedsPassword.Contains((int)(account.AccountType));
        }
        
    }
}