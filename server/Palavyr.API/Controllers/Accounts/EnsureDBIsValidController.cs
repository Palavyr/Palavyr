using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Data;
using Palavyr.Services.DatabaseService;
using Palavyr.Services.StripeServices;

namespace Palavyr.API.Controllers.Accounts
{
    [Route("api")]
    [ApiController]
    public class EnsureDbIsValidController : ControllerBase
    {
        private readonly IAccountsConnector accountsConnector;
        private readonly IDashConnector dashConnector;
        private ILogger<DeleteAccountController> logger;
        private DashContext dashContext;
        private AccountsContext accountsContext;
        private StripeCustomerService stripeCustomerService;

        public EnsureDbIsValidController(
            IAccountsConnector accountsConnector,
            IDashConnector dashConnector,
            ILogger<DeleteAccountController> logger,
            AccountsContext accountsContext,
            DashContext dashContext,
            StripeCustomerService stripeCustomerService

            )
        {
            this.accountsConnector = accountsConnector;
            this.dashConnector = dashConnector;
            this.logger = logger;
            this.dashContext = dashContext;
            this.accountsContext = accountsContext;
            this.stripeCustomerService = stripeCustomerService;
        }

        [HttpPost("configure-conversations/ensure-db-valid")]
        public async Task<NoContentResult> Ensure([FromHeader] string accountId)
        {
            var preferences = await dashConnector.GetWidgetPreferences(accountId);
            var account = await accountsConnector.GetAccount(accountId);
 
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

            await dashConnector.CommitChangesAsync();
            return NoContent();
        }
    }
}