using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{

    public class ModifyAreaEmailTemplateController : PalavyrBaseController
    {
        private DashContext dashContext;
        private ILogger<ModifyAreaEmailTemplateController> logger;

        public ModifyAreaEmailTemplateController(
            ILogger<ModifyAreaEmailTemplateController> logger,
            DashContext dashContext
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpPut("email/{areaId}/email-template")]
        public async Task<string> Modify([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] EmailTemplateRequest request)
        {
            var currentArea = dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Single(row => row.AreaIdentifier == areaId);
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