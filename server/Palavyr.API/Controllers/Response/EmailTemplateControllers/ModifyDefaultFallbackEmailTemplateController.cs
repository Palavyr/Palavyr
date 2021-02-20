using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{
    [Route("api")]
    [ApiController]
    public class ModifyDefaultFallbackEmailTemplateController : ControllerBase
    {
        private AccountsContext accountContext;
        private ILogger<ModifyDefaultFallbackEmailTemplateController> logger;

        public ModifyDefaultFallbackEmailTemplateController(
            ILogger<ModifyDefaultFallbackEmailTemplateController> logger,
            AccountsContext accountContext
        )
        {
            this.logger = logger;
            this.accountContext = accountContext;
        }

        [HttpPut("email/fallback/default-email-template")]
        public async Task<string> Modify([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] DefaultEmailTemplateRequest request)
        {
            var account = await accountContext.Accounts.SingleAsync(row => row.AccountId == accountId);
            account.GeneralFallbackEmailTemplate = request.EmailTemplate;
            await accountContext.SaveChangesAsync();
            return account.GeneralFallbackEmailTemplate;
        }
    }

    public class DefaultEmailTemplateRequest
    {
        public string EmailTemplate { get; set; }
    }
}