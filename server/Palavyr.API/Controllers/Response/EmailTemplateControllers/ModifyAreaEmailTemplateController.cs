using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{

    public class ModifyAreaEmailTemplateController : PalavyrBaseController
    {
        private DashContext dashContext;
        private ILogger<ModifyAreaEmailTemplateController> logger;
        private IConfigurationRepository configurationRepository;

        public ModifyAreaEmailTemplateController(
            ILogger<ModifyAreaEmailTemplateController> logger,
            IConfigurationRepository configurationRepository
        )
        {
            this.logger = logger;
            this.configurationRepository = configurationRepository;
        }

        [HttpPut("email/{areaId}/email-template")]
        public async Task<string> Modify([FromRoute] string areaId, [FromBody] EmailTemplateRequest request)
        {
            var currentArea = await configurationRepository.GetAreaById(areaId);
            currentArea.EmailTemplate = request.EmailTemplate;
            await dashContext.SaveChangesAsync();
            return currentArea.EmailTemplate;
        }
    }

    public class EmailTemplateRequest
    {
        public string EmailTemplate { get; set; }
    }
}