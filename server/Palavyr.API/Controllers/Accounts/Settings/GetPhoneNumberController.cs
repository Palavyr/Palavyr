using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class GetPhoneNumberController : PalavyrBaseController
    {
        private AccountsContext accountsContext;
        private ILogger<GetPhoneNumberController> logger;
        public GetPhoneNumberController(AccountsContext accountsContext, ILogger<GetPhoneNumberController> logger)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }
        
        [HttpGet("account/settings/phone-number")]
        public async Task<IActionResult> Get([FromHeader] string accountId)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            var phoneDetails = PhoneDetails.Create(account.PhoneNumber, account.Locale);
            return Ok(phoneDetails);
        }
    }
}