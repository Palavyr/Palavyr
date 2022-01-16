using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{

    public class ModifyDefaultFallbackEmailTemplateController : PalavyrBaseController
    {
        private ILogger<ModifyDefaultFallbackEmailTemplateController> logger;
        private readonly IAccountRepository accountRepository;

        public ModifyDefaultFallbackEmailTemplateController(
            ILogger<ModifyDefaultFallbackEmailTemplateController> logger,
            IAccountRepository accountRepository
        )
        {
            this.logger = logger;
            this.accountRepository = accountRepository;
        }

        [HttpPut("email/fallback/default-email-template")]
        public async Task<string> Modify([FromRoute] string areaId, [FromBody] DefaultEmailTemplateRequest request)
        {
            var account = await accountRepository.GetAccount();
            account.GeneralFallbackEmailTemplate = request.EmailTemplate;
            await accountRepository.CommitChangesAsync();
            return account.GeneralFallbackEmailTemplate;
        }
    }

    public class DefaultEmailTemplateRequest
    {
        public string EmailTemplate { get; set; }
    }
}