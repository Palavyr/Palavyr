using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Web.Http.Cors;
using DashboardServer.API.ReceiverTypes;
using Microsoft.AspNetCore.Mvc;


namespace DashboardServer.API.Controllers
{
    // [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")] 
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