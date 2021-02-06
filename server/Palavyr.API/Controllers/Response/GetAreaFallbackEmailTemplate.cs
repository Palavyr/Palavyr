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
    public class GetAreaFallbackEmailTemplate : ControllerBase
    {
        private ILogger<GetAreaFallbackEmailTemplate> logger;
        private DashContext dashContext;

        public GetAreaFallbackEmailTemplate(DashContext dashContext, ILogger<GetAreaFallbackEmailTemplate> logger)
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
        [HttpGet("email/{areaId}/fallback-email-template")]
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