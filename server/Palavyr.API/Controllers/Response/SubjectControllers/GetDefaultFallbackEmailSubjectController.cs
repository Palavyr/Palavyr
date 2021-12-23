using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    public class GetDefaultFallbackEmailSubjectController : PalavyrBaseController
    {
        private AccountsContext accountsContext;
        private ILogger<GetDefaultFallbackEmailSubjectController> logger;
        private readonly IAccountRepository accountRepository;

        public GetDefaultFallbackEmailSubjectController(
            ILogger<GetDefaultFallbackEmailSubjectController> logger,
            IAccountRepository accountRepository
        )
        {
            this.logger = logger;
            this.accountRepository = accountRepository;
        }

        [HttpGet("email/default-fallback-subject")]
        public async Task<string> Modify([FromRoute] string areaId)
        {
            var account = await accountRepository.GetAccount(); 
            var subject = account.GeneralFallbackSubject;
            return subject;
        }
    }
}