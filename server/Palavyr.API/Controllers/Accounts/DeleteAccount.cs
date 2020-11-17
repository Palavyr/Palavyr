using System;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Palavyr.API.Controllers.Accounts
{
    
    [Route("api/account")]
    [ApiController]
    public class DeleteAccountController : BaseController
    {
        private static ILogger<DeleteAccountController> _logger;
        private readonly IStripeClient _stripeClient = new StripeClient(StripeConfiguration.ApiKey);

        public DeleteAccountController(
            ILogger<DeleteAccountController> logger,
            AccountsContext accountContext, 
            ConvoContext convoContext,
            DashContext dashContext, 
            IWebHostEnvironment env
            ) : base(accountContext, convoContext, dashContext, env)
        {
            _logger = logger;
        }

        [HttpPost("delete-account")]
        public async Task<IActionResult> DeleteAccount([FromHeader] string accountId)
        {

            var service = new CustomerService();
            var account = AccountContext.Accounts.SingleOrDefault(row => row.AccountId == accountId);
            if (account == null) throw new Exception("Invalid Account State detected no delete");
            var stripeCustomerId = account.StripeCustomerId;
            await service.DeleteAsync(stripeCustomerId);
            
            // Convo Database
            ConvoContext.Conversations.RemoveRange(ConvoContext.Conversations.Where(row => row.AccountId == accountId));
            ConvoContext.CompletedConversations.RemoveRange(ConvoContext.CompletedConversations.Where(row => row.AccountId == accountId));

            // configuration db
            DashContext.Areas.RemoveRange(DashContext.Areas.Where(row => row.AccountId == accountId));
            DashContext.ConversationNodes.RemoveRange(DashContext.ConversationNodes.Where(row => row.AccountId == accountId));
            DashContext.DynamicTableMetas.RemoveRange(DashContext.DynamicTableMetas.Where(row => row.AccountId == accountId));
            DashContext.FileNameMaps.RemoveRange(DashContext.FileNameMaps.Where(row => row.AccountId == accountId));
            DashContext.Groups.RemoveRange(DashContext.Groups.Where(row => row.AccountId == accountId));
            DashContext.SelectOneFlats.RemoveRange(DashContext.SelectOneFlats.Where(row => row.AccountId == accountId));
            DashContext.StaticFees.RemoveRange(DashContext.StaticFees.Where(row => row.AccountId == accountId));
            DashContext.StaticTablesMetas.RemoveRange(DashContext.StaticTablesMetas.Where(row => row.AccountId == accountId));
            DashContext.StaticTablesRows.RemoveRange(DashContext.StaticTablesRows.Where(row => row.AccountId == accountId));
            DashContext.WidgetPreferences.RemoveRange(DashContext.WidgetPreferences.Where(row => row.AccountId == accountId));
            
            // lastly, convo db
            AccountContext.Accounts.RemoveRange(AccountContext.Accounts.Where(row => row.AccountId == accountId));
            AccountContext.EmailVerifications.RemoveRange(AccountContext.EmailVerifications.Where(row => row.AccountId == accountId));
            AccountContext.Sessions.RemoveRange(AccountContext.Sessions.Where(row => row.AccountId == accountId));
            AccountContext.Subscriptions.RemoveRange(AccountContext.Subscriptions.Where(row => row.AccountId == accountId));
            
            await ConvoContext.SaveChangesAsync();
            await DashContext.SaveChangesAsync();
            await AccountContext.SaveChangesAsync();

            return Ok();
        }
        
    }
}