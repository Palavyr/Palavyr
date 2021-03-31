using System;
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
        private readonly LocaleDefinition localeDefinition;
        private AccountsContext accountsContext;

        public ModifyLocaleController(AccountsContext accountsContext, ILogger<ModifyLocaleController> logger, LocaleDefinition localeDefinition)
        {
            this.logger = logger;
            this.localeDefinition = localeDefinition;
            this.accountsContext = accountsContext;
        }

        [HttpPut("account/settings/locale")]
        public async Task<LocaleDefinition> Modify([FromHeader] string accountId, [FromBody] RequestBody request)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);

            var newLocale = localeDefinition.Parse(request.LocaleId);
            if (!newLocale.IsValidLocal())
            {
                throw new Exception($"Locale {request.LocaleId} is not supported. Supported locales: {string.Join(", ", newLocale.GetSupportedLocales)}");
            }

            account.Locale = newLocale.LocaleId;
            await accountsContext.SaveChangesAsync();
            return newLocale;
        }

        public class RequestBody
        {
            public string LocaleId { get; set; }
        }
    }
}