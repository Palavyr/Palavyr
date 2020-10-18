using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.ReceiverTypes;
using Palavyr.API.response;


namespace Palavyr.API.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailTemplateController : BaseController
    {
        public EmailTemplateController(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext, IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env) { }
        
        [HttpGet("{areaId}/emailTemplate")]
        public string GetEmailTemplate([FromHeader] string accountId, string areaId)
        {
            var emailTemplate = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Single(row => row.AreaIdentifier == areaId)
                .EmailTemplate;
            
            // Substitute Variables
            const string clientName = "[Insert Name]";
            var companyName = AccountContext.Accounts.SingleOrDefault(row => row.AccountId == accountId).CompanyName;
            var logoUri = AccountContext.Accounts.SingleOrDefault(row => row.AccountId == accountId).AccountLogoUri;
            emailTemplate = SupportedSubstitutions.MakeVariableSubstitutions(emailTemplate, companyName, clientName, logoUri);
            return emailTemplate;
        }

        [HttpPut("{areaId}/emailTemplate")]
        public string SaveEmailTemplate([FromHeader] string accountId, string areaId, [FromBody] Text text)
        {
            var currentArea = DashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Single(row => row.AreaIdentifier == areaId);
            currentArea.EmailTemplate = text.EmailTemplate;
            DashContext.SaveChanges();
            return currentArea.EmailTemplate;
        }
    }
}