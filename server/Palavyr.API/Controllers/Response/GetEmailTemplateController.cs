using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.response;

namespace Palavyr.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class GetEmailTemplateController : ControllerBase
    {
        private ILogger<GetEmailTemplateController> logger;
        private DashContext dashContext;
        private AccountsContext accountsContext;

        public GetEmailTemplateController(AccountsContext accountsContext, DashContext dashContext, ILogger<GetEmailTemplateController> logger)
        {
            this.accountsContext = accountsContext;
            this.dashContext = dashContext;
            this.logger = logger;
        }
        
        [HttpGet("email/{areaId}/emailTemplate")]
        public async Task<IActionResult> GetEmailTemplate([FromHeader] string accountId, string areaId)
        {
            var record = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .SingleOrDefaultAsync(row => row.AreaIdentifier == areaId);
            var emailTemplate = record.EmailTemplate;
            
            // Substitute Variables
            const string clientName = "[Insert Name]";
            var companyName = (
                await accountsContext.
                    Accounts.
                    SingleOrDefaultAsync(row => row.AccountId == accountId)
            ).CompanyName;
            var logoUri = (
                await accountsContext
                    .Accounts
                    .SingleOrDefaultAsync(row => row.AccountId == accountId)
            ).AccountLogoUri;
            emailTemplate = ResponseVariableSubstitution.MakeVariableSubstitutions(emailTemplate, companyName, clientName, logoUri);
            return Ok(emailTemplate);
        }
        
    }
}