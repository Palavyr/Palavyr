using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Services.Repositories.Delete;

namespace Palavyr.API.Controllers.Accounts
{
    public class DeleteAccountController : PalavyrBaseController
    {
        private readonly IAccountDeleter accountDeleter;
        private readonly IDashDeleter dashDeleter;
        private readonly IConvoDeleter convoDeleter;
        private ILogger<DeleteAccountController> logger;

        public DeleteAccountController(
            IAccountDeleter accountDeleter,
            IDashDeleter dashDeleter,
            IConvoDeleter convoDeleter,
            ILogger<DeleteAccountController> logger
        )
        {
            this.accountDeleter = accountDeleter;
            this.dashDeleter = dashDeleter;
            this.convoDeleter = convoDeleter;
            this.logger = logger;
        }

        [HttpPost("account/delete-account")]
        public async Task<IActionResult> DeleteAccount([FromHeader] string accountId)
        {
            logger.LogInformation($"Deleting details for account: {accountId}");

            logger.LogInformation("Deleting from the convo database...");
            convoDeleter.DeleteAccount(accountId);

            logger.LogInformation("Deleting from the dash database...");
            dashDeleter.DeleteAccount(accountId);

            logger.LogDebug("Deleting from the Accounts database...");
            await accountDeleter.DeleteAccount(accountId);

            await accountDeleter.CommitChangesAsync();
            await dashDeleter.CommitChangesAsync();
            await convoDeleter.CommitChangesAsync();
            return Ok();
        }
    }
}