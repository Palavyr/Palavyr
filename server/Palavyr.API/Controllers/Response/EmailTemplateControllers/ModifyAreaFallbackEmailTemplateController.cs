using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{

    public class ModifyAreaFallbackEmailTemplateController : PalavyrBaseController
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

        [HttpPut("email/fallback/{areaId}/email-template")]
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