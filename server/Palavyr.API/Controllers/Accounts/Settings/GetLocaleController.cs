using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetLocaleController : PalavyrBaseController
    {
        private AccountsContext accountsContext;
        private ILogger<GetLocaleController> logger;
        private readonly LocaleDefinitions localeDefinitions;

        public GetLocaleController(AccountsContext accountsContext, ILogger<GetLocaleController> logger, LocaleDefinitions localeDefinitions)
        {
            this.logger = logger;
            this.localeDefinitions = localeDefinitions;
            this.accountsContext = accountsContext;
        }


        [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
        [HttpGet("account/settings/locale/widget")]
        public async Task<LocaleResponse> GetForWidget(
            [FromHeader]
            string accountId)
        {
            // var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            // var localeMeta = localeDefinitions.Parse(account.Locale);
            // return localeMeta;
            var localeResponse = await GetLocaleResponse(accountId);
            return localeResponse;
        }

        [HttpGet("account/settings/locale")]
        public async Task<LocaleResponse> Get(
            [FromHeader]
            string accountId)
        {
            var localeResponse = await GetLocaleResponse(accountId);
            return localeResponse;
        }

        private async Task<LocaleResponse> GetLocaleResponse(string accountId)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            var localeMeta = localeDefinitions.Parse(account.Locale);
            var culture = new CultureInfo(localeMeta.Name);

            return new LocaleResponse
            {
                CurrentLocale = localeMeta,
                LocaleMap = culture.CreateLocaleMap()
            };
        }
    }

    public class LocaleResponse
    {
        public LocaleResource CurrentLocale { get; set; }
        public LocaleResource[] LocaleMap { get; set; }
    }
}