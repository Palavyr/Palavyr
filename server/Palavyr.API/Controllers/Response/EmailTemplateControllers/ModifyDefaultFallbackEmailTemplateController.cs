using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{

    public class ModifyDefaultFallbackEmailTemplateController : PalavyrBaseController
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