using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;

namespace Palavyr.API.Controllers.Response
{
    [Route("api")]
    [ApiController]
    public class ModifyAreaFallbackEmailTemplateController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<ModifyAreaFallbackEmailTemplateController> logger;

        public ModifyAreaFallbackEmailTemplateController(
            ILogger<ModifyAreaFallbackEmailTemplateController> logger,
            DashContext dashContext
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpPut("email/{areaId}/fallback-email-template")]
        public async Task<string> Modify([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] FallbackEmailTemplateRequest request)
        {
            var currentArea = dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Single(row => row.AreaIdentifier == areaId);
            currentArea.FallbackEmailTemplate = request.EmailTemplate;
            await dashContext.SaveChangesAsync();
            return currentArea.FallbackEmailTemplate;
        }

    }

    public class FallbackEmailTemplateRequest
    {
        public string EmailTemplate { get; set; }
    }
}