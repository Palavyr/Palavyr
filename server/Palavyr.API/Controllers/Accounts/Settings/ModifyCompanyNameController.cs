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

    public class ModifyCompanyNameController : PalavyrBaseController
    {
        private readonly IAccountRepository accountRepository;
        private ILogger<ModifyCompanyNameController> logger;

        public ModifyCompanyNameController(IAccountRepository  accountRepository, ILogger<ModifyCompanyNameController> logger)
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
        }
        
        [HttpPut("account/settings/company-name")]
        public async Task<string> Modify(CompanyNameSettingsRequest settingsRequest)
        {
            var account = await accountRepository.GetAccount();
            account.CompanyName = settingsRequest.CompanyName;
            await accountRepository.CommitChangesAsync();
            return account.CompanyName;
        }
    }
}