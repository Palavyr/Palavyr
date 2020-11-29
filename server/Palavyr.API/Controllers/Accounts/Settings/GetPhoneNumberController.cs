using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.ResponseTypes;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    [Route("api")]
    [ApiController]
    public class GetPhoneNumberController : ControllerBase
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