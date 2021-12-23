using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class ModifyPhoneNumberController : PalavyrBaseController
    {
        private readonly IAccountRepository accountRepository;
        private ILogger<ModifyPhoneNumberController> logger;

        public ModifyPhoneNumberController(IAccountRepository accountRepository, ILogger<ModifyPhoneNumberController> logger)
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
        }
        
        [HttpPut("account/settings/phone-number")]
        public async Task<string> Modify(PhoneNumberSettingsRequest settings)
        {
            var account = await accountRepository.GetAccount();
            account.PhoneNumber = settings.PhoneNumber ?? "";
            await accountRepository.CommitChangesAsync();
            return account.PhoneNumber;
        }
    }
}