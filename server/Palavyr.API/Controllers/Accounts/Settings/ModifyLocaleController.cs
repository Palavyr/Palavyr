using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class ModifyLocaleController : PalavyrBaseController
    {
        private readonly IAccountRepository accountRepository;
        private ILogger<ModifyLocaleController> logger;
        private readonly LocaleDefinitions localeDefinitions;

        public ModifyLocaleController(IAccountRepository accountRepository, ILogger<ModifyLocaleController> logger, LocaleDefinitions localeDefinitions)
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
            this.localeDefinitions = localeDefinitions;
        }

        [HttpPut("account/settings/locale")]
        public async Task<LocaleResponse> Modify(
            [FromBody]
            RequestBody request)
        {
            var account = await accountRepository.GetAccount();
            var newLocale = localeDefinitions.Parse(request.LocaleId);

            account.Locale = newLocale.Name;
            await accountRepository.CommitChangesAsync();
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