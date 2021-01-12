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
    public class ModifyEmailTemplateController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<ModifyEmailTemplateController> logger;

        public ModifyEmailTemplateController(
            ILogger<ModifyEmailTemplateController> logger,
            DashContext dashContext
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpPut("email/{areaId}/emailTemplate")]
        public async Task<string> Modify([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] Text text)
        {
            var currentArea = dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Single(row => row.AreaIdentifier == areaId);
            currentArea.EmailTemplate = text.EmailTemplate;
            await dashContext.SaveChangesAsync();
            return currentArea.EmailTemplate;
        }
    }
}