using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.StripeServices;

namespace Palavyr.API.Controllers.Accounts
{
    [Route("api")]
    [ApiController]
    public class EnsureDbIsValidController : ControllerBase
    {
        
        private ILogger<DeleteAccountController> logger;
        private DashContext dashContext;
        private AccountsContext accountsContext;
        private StripeCustomerService stripeCustomerService;

        public EnsureDbIsValidController(
            ILogger<DeleteAccountController> logger,
            AccountsContext accountsContext,
            DashContext dashContext,
            StripeCustomerService stripeCustomerService

            )
        {
            this.logger = logger;
            this.dashContext = dashContext;
            this.accountsContext = accountsContext;
            this.stripeCustomerService = stripeCustomerService;
        }

        [HttpPost("configure-conversations/ensure-db-valid")]
        public async Task<NoContentResult> Ensure([FromHeader] string accountId)
        {

            var preferences = dashContext.WidgetPreferences.Single(row => row.AccountId == accountId);
            var account = accountsContext.Accounts.Single(row => row.AccountId == accountId);

            if (string.IsNullOrWhiteSpace(account.StripeCustomerId))
            {
                var newCustomer = await stripeCustomerService.CreateNewStripeCustomer(account.EmailAddress);
                account.StripeCustomerId = newCustomer.Id;
                await accountsContext.SaveChangesAsync();
            }

            if (string.IsNullOrWhiteSpace(preferences.ChatBubbleColor))
                preferences.ChatBubbleColor = "#E1E1E1";

            if (string.IsNullOrWhiteSpace(preferences.ChatFontColor))
                preferences.ChatFontColor = "#35CCE6";

            if (string.IsNullOrWhiteSpace(preferences.OptionsHeaderColor))
                preferences.OptionsHeaderColor = "#35CCE6";

            if (string.IsNullOrWhiteSpace(preferences.OptionsHeaderFontColor))
                preferences.OptionsHeaderFontColor = "#35CCE6";

            if (string.IsNullOrWhiteSpace(preferences.ListFontColor))
                preferences.ListFontColor = "#35CCE6";

            if (string.IsNullOrWhiteSpace(preferences.HeaderFontColor))
                preferences.HeaderFontColor = "#35CCE6";

            if (string.IsNullOrWhiteSpace(preferences.SelectListColor))
                preferences.SelectListColor = "#E1E1E1";

            if (string.IsNullOrWhiteSpace(preferences.HeaderColor))
                preferences.HeaderColor = "#35CCE6";

            if (string.IsNullOrWhiteSpace(preferences.FontFamily))
                preferences.FontFamily = "Architects Daughter";
            
            await dashContext.SaveChangesAsync();
            
            return NoContent();
        }
    }
}