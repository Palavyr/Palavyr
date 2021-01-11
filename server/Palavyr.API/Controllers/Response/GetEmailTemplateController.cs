using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.Response
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
        
        /// <summary>
        /// This controller should only be used for getting the template for the editor. I.e. NO variable substitution.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet("email/{areaId}/emailTemplate")]
        public async Task<string> GetEmailTemplate([FromHeader] string accountId, string areaId)
        {
            var record = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .SingleOrDefaultAsync(row => row.AreaIdentifier == areaId);
            var emailTemplate = record.EmailTemplate;
            return emailTemplate;
        }
        
    }
}