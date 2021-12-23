using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response.EmailTemplateControllers
{

    public class GetDefaultFallbackEmailTemplateController : PalavyrBaseController
    {
        private ILogger<GetDefaultFallbackEmailTemplateController> logger;
        private readonly IAccountRepository accountRepository;

        public GetDefaultFallbackEmailTemplateController(
            ILogger<GetDefaultFallbackEmailTemplateController> logger,
            IAccountRepository accountRepository
        )
        {
            this.logger = logger;
            this.accountRepository = accountRepository;
        }

        [HttpGet("email/fallback/default-email-template")]
        public async Task<string> Modify([FromRoute] string areaId)
        {
            var account = await accountRepository.GetAccount();
            var currentDefaultEmailTemplate = account.GeneralFallbackEmailTemplate;
            return currentDefaultEmailTemplate;
        }
    }
}
