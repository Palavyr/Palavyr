using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{

    public class ModifyAreaFallbackEmailTemplateController : PalavyrBaseController
    {
        private DashContext dashContext;
        private readonly IConfigurationRepository repository;
        private ILogger<ModifyAreaFallbackEmailTemplateController> logger;

        public ModifyAreaFallbackEmailTemplateController(
            ILogger<ModifyAreaFallbackEmailTemplateController> logger,
            DashContext dashContext,
            IConfigurationRepository repository
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
            this.repository = repository;
        }

        [HttpPut("email/fallback/{areaId}/email-template")]
        public async Task<string> Modify([FromRoute] string areaId, [FromBody] FallbackEmailTemplateRequest request)
        {
            var currentArea = await repository.GetAreaById(areaId);
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