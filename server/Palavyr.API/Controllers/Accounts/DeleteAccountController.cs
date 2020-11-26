using System;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.StripeServices;

namespace Palavyr.API.Controllers.Accounts
{
    [Route("api")]
    [ApiController]
    public class DeleteAccountController : ControllerBase
    {
        private ILogger<DeleteAccountController> logger;
        private DashContext dashContext;
        private ConvoContext convoContext;
        private AccountsContext accountsContext;
        private IStripeCustomerService stripeCustomerService;

        public DeleteAccountController(
            ILogger<DeleteAccountController> logger,
            AccountsContext accountsContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IStripeCustomerService stripeCustomerService
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
            this.convoContext = convoContext;
            this.accountsContext = accountsContext;
            this.stripeCustomerService = stripeCustomerService;
        }

        [HttpPost("account/delete-account")]
        public async Task<IActionResult> DeleteAccount([FromHeader] string accountId)
        {
            logger.LogDebug($"0. Deleting details for account: {accountId}");
            logger.LogDebug("1. Collecting account");
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            if (account == null)
            {
                throw new Exception("Invalid Account State detected no delete");
            }

            if (account.StripeCustomerId == null)
            {
                throw new Exception("Stripe Customer ID not set in database");
            }

            await stripeCustomerService.DeleteSingleLiveStripeCustomer(account.StripeCustomerId);
            
            logger.LogDebug("2. Deleting Convo Database...");
            // Convo Database
            convoContext.Conversations.RemoveRange(convoContext.Conversations.Where(row => row.AccountId == accountId));
            convoContext.CompletedConversations.RemoveRange(
                convoContext.CompletedConversations.Where(row => row.AccountId == accountId));

            logger.LogDebug("3. Deleting Configuration DB");
            // configuration db
            dashContext.Areas.RemoveRange(dashContext.Areas.Where(row => row.AccountId == accountId));
            dashContext.ConversationNodes.RemoveRange(
                dashContext.ConversationNodes.Where(row => row.AccountId == accountId));
            dashContext.DynamicTableMetas.RemoveRange(
                dashContext.DynamicTableMetas.Where(row => row.AccountId == accountId));
            dashContext.FileNameMaps.RemoveRange(dashContext.FileNameMaps.Where(row => row.AccountId == accountId));

            logger.LogDebug("3.5 Half way through Configuration DB");
            dashContext.Groups.RemoveRange(dashContext.Groups.Where(row => row.AccountId == accountId));
            dashContext.SelectOneFlats.RemoveRange(dashContext.SelectOneFlats.Where(row => row.AccountId == accountId));
            dashContext.StaticFees.RemoveRange(dashContext.StaticFees.Where(row => row.AccountId == accountId));
            dashContext.StaticTablesMetas.RemoveRange(
                dashContext.StaticTablesMetas.Where(row => row.AccountId == accountId));
            dashContext.StaticTablesRows.RemoveRange(
                dashContext.StaticTablesRows.Where(row => row.AccountId == accountId));
            dashContext.WidgetPreferences.RemoveRange(
                dashContext.WidgetPreferences.Where(row => row.AccountId == accountId));

            logger.LogDebug("4. Deleting Accounts DB");
            // lastly, accounts db
            accountsContext.Accounts.RemoveRange(accountsContext.Accounts.Where(row => row.AccountId == accountId));
            accountsContext.EmailVerifications.RemoveRange(
                accountsContext.EmailVerifications.Where(row => row.AccountId == accountId));
            accountsContext.Sessions.RemoveRange(accountsContext.Sessions.Where(row => row.AccountId == accountId));
            accountsContext.Subscriptions.RemoveRange(
                accountsContext.Subscriptions.Where(row => row.AccountId == accountId));

            logger.LogDebug("5. Saving DB Deletion Changes.");
            await convoContext.SaveChangesAsync();
            await dashContext.SaveChangesAsync();
            await accountsContext.SaveChangesAsync();

            return Ok();
        }
    }
}