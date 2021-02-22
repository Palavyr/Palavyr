using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{
    [Route("api")]
    [ApiController]
    public class GetAreaFallbackEmailTemplateController : ControllerBase
    {
        private ILogger<GetAreaFallbackEmailTemplateController> logger;
        private DashContext dashContext;

        public GetAreaFallbackEmailTemplateController(DashContext dashContext, ILogger<GetAreaFallbackEmailTemplateController> logger)
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
        [HttpGet("email/fallback/{areaId}/email-template")]
        public async Task<string> Get([FromHeader] string accountId, string areaId)
        {
            var area = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .SingleOrDefaultAsync(row => row.AreaIdentifier == areaId);
            var emailTemplate = area.FallbackEmailTemplate;
            return emailTemplate;
        }
        
    }
}