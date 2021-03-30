using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;
using Palavyr.Services.AccountServices;
using Palavyr.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class GetLocaleController : PalavyrBaseController
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