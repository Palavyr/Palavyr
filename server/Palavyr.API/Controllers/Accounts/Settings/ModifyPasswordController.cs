using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Requests.Registration;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class ModifyPasswordController : PalavyrBaseController
    {
        private readonly IAccountRepository accountRepository;
        private ILogger<ModifyPasswordController> logger;

        public ModifyPasswordController(IAccountRepository accountRepository, ILogger<ModifyPasswordController> logger)
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
        }
        
        [HttpPut("account/settings/password")]
        public async Task<bool> UpdatePassword(AccountDetails accountDetails)
        {
            var account = await accountRepository.GetAccount();
            var oldHashedPassword = accountDetails.OldPassword;
            if (oldHashedPassword != accountDetails.Password)
            {
                return false;
            }

            account.Password = accountDetails.Password;
            await accountRepository.CommitChangesAsync();
            return true;
        }
    }
}