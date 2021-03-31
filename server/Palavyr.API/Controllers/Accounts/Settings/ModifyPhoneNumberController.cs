using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Requests;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class ModifyPhoneNumberController : PalavyrBaseController
    {
        private ILogger<ModifyPhoneNumberController> logger;
        private AccountsContext accountsContext;

        public ModifyPhoneNumberController(AccountsContext accountsContext, ILogger<ModifyPhoneNumberController> logger)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }
        
        [HttpPut("account/settings/phone-number")]
        public async Task<string> Modify([FromHeader] string accountId, LoginCredentials login)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            account.PhoneNumber = login.PhoneNumber;
            await accountsContext.SaveChangesAsync();
            return account.PhoneNumber;
        }
    }
}