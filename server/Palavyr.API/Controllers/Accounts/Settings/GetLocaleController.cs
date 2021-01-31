using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.AccountServices;

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
        
        [HttpGet("account/settings/locale")]
        public async Task<LocaleDefinition> Get([FromHeader] string accountId)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            var localeMeta = localeDefinition.Parse(account.Locale);
            return localeMeta;
        }
    }


}