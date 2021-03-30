using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{

    public class GetAreaEmailTemplateController : PalavyrBaseController
    {
        private ILogger<GetAreaEmailTemplateController> logger;
        private DashContext dashContext;

        public GetAreaEmailTemplateController(DashContext dashContext, ILogger<GetAreaEmailTemplateController> logger)
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }
        
        /// <summary>
        /// This controller should only be used for getting the template for the editor. I.e. NO variable substitution.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet("email/{areaId}/email-template")]
        public async Task<string> GetEmailTemplate([FromHeader] string accountId, string areaId)
        {
            var area = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .SingleOrDefaultAsync(row => row.AreaIdentifier == areaId);
            var emailTemplate = area.EmailTemplate;
            return emailTemplate;
        }
        
    }
}