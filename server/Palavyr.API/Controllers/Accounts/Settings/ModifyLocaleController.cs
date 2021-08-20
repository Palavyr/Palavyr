using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class ModifyLocaleController : PalavyrBaseController
    {
        private ILogger<ModifyLocaleController> logger;
        private readonly LocaleDefinitions localeDefinitions;
        private AccountsContext accountsContext;

        public ModifyLocaleController(AccountsContext accountsContext, ILogger<ModifyLocaleController> logger, LocaleDefinitions localeDefinitions)
        {
            this.logger = logger;
            this.localeDefinitions = localeDefinitions;
            this.accountsContext = accountsContext;
        }

        [HttpPut("account/settings/locale")]
        public async Task<LocaleResponse> Modify(
            [FromHeader]
            string accountId,
            [FromBody]
            RequestBody request)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);

            var newLocale = localeDefinitions.Parse(request.LocaleId);

            account.Locale = newLocale.Name;
            await accountsContext.SaveChangesAsync();
            var culture = new CultureInfo(newLocale.Name);

            return new LocaleResponse
            {
                CurrentLocale = culture.ConvertToResource(),
                LocaleMap = culture.CreateLocaleMap()
            };
        }

        public class RequestBody
        {
            public string LocaleId { get; set; }
        }
    }
}