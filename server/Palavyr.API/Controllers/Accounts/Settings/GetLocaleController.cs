using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetLocaleController : PalavyrBaseController
    {
        private readonly IAccountRepository accountRepository;
        private ILogger<GetLocaleController> logger;
        private readonly LocaleDefinitions localeDefinitions;

        public GetLocaleController(IAccountRepository accountRepository, ILogger<GetLocaleController> logger, LocaleDefinitions localeDefinitions)
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
            this.localeDefinitions = localeDefinitions;
        }


        [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
        [HttpGet("account/settings/locale/widget")]
        public async Task<LocaleResponse> GetForWidget(CancellationToken cancellationToken)
        {
            var localeResponse = await GetLocaleResponse();
            return localeResponse;
        }

        [HttpGet("account/settings/locale")]
        public async Task<LocaleResponse> Get(CancellationToken cancellationToken)
        {
            var localeResponse = await GetLocaleResponse();
            return localeResponse;
        }

        private async Task<LocaleResponse> GetLocaleResponse()
        {
            var account = await accountRepository.GetAccount();
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