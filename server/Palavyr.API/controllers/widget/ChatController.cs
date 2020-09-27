using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;
using Server.Domain;
using Server.Domain.conversations;

namespace Palavyr.API.Controllers
{
    // [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")] 
    [Route("api/widget")]
    [ApiController]
    public class ChatController : BaseController
    {
        public ChatController(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext, IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env) { }

        private string GetAccountId(string apiKey)
        {
            var account = AccountContext.Accounts.Single(row => row.ApiKey == apiKey);
            return account.AccountId;
        }
        
        [HttpPost("{apiKey}/conversation")]
        public StatusCodeResult UpdateActiveConversation(string apiKey, ConversationUpdate update)
        {
            var accountId = GetAccountId(apiKey);
            var conversationUpdate = update.CreateFromPartial(accountId);
            ConvoContext.Conversations.Add(conversationUpdate);
            ConvoContext.SaveChanges();
            return new OkResult();
        }
    }
}