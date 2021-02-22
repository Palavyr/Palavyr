using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes.Registration;
using Palavyr.Data.Abstractions;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    [Route("api")]
    [ApiController]
    public class ModifyPasswordController : ControllerBase
    {
        private readonly IAccountsConnector accountsConnector;
        private ILogger<ModifyPasswordController> logger;

        public ModifyPasswordController(IAccountsConnector accountsConnector, ILogger<ModifyPasswordController> logger)
        {
            this.accountsConnector = accountsConnector;
            this.logger = logger;
        }
        
        [HttpPut("account/settings/password")]
        public async Task<bool> UpdatePassword([FromHeader] string accountId, AccountDetails accountDetails)
        {
            var account = await accountsConnector.GetAccount(accountId);
            var oldHashedPassword = accountDetails.OldPassword;
            if (oldHashedPassword != accountDetails.Password)
            {
                return false;
            }

            account.Password = accountDetails.Password;
            await accountsConnector.CommitChanges();
            return true;
        }
    }
}