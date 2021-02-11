using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.AccountServices;
using Microsoft.AspNetCore.Authorization;
using Palavyr.API.Services.AuthenticationServices;


namespace Palavyr.API.Controllers.Accounts.Settings
{
    [Route("api")]
    [ApiController]
    public class GetLocaleController : ControllerBase
    {
        private AccountsContext accountsContext;
        private ILogger<GetLocaleController> logger;
        private readonly LocaleDefinition localeDefinition;

        public GetLocaleController(AccountsContext accountsContext, ILogger<GetLocaleController> logger, LocaleDefinition localeDefinition)
        {
            this.logger = logger;
            this.localeDefinition = localeDefinition;
            this.accountsContext = accountsContext;
        }


        [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
        [HttpGet("account/settings/locale/widget")]
        public async Task<LocaleDefinition> GetForWidget([FromHeader] string accountId)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            var localeMeta = localeDefinition.Parse(account.Locale);
            return localeMeta;
        }

        [HttpGet("account/settings/locale")]
        public async Task<LocaleDefinition> Get([FromHeader] string accountId)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            var localeMeta = localeDefinition.Parse(account.Locale);
            return localeMeta;
        }
    }
}