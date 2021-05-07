using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories.Delete;

namespace Palavyr.API.Controllers.Accounts
{
    public class DeleteDevDataByAccountIdController : PalavyrBaseController
    {
        private readonly ILogger<DeleteDevDataByAccountIdController> logger;
        private readonly IAccountDeleter accountDeleter;
        private readonly IDashDeleter dashDeleter;
        private readonly IConvoDeleter convoDeleter;

        public DeleteDevDataByAccountIdController(
            ILogger<DeleteDevDataByAccountIdController> logger,
            IAccountDeleter accountDeleter,
            IDashDeleter dashDeleter,
            IConvoDeleter convoDeleter
        )
        {
            this.logger = logger;
            this.accountDeleter = accountDeleter;
            this.dashDeleter = dashDeleter;
            this.convoDeleter = convoDeleter;
        }

        [HttpDelete("dev/{devKey}/{accountId}")]
        public async Task Delete(string accountId, string devKey)
        {
            if (devKey != "secretTobyface")
            {
                logger.LogDebug("This is an attempt to Refresh database data.");
                return;
            }
            
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
        }
    }
}