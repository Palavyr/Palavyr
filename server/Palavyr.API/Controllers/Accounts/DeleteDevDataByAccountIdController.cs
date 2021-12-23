using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Repositories.Delete;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.Accounts
{
    public class DeleteDevDataByAccountIdController : PalavyrBaseController
    {
        private readonly ILogger<DeleteDevDataByAccountIdController> logger;
        private readonly IAccountDeleter accountDeleter;
        private readonly IDashDeleter dashDeleter;
        private readonly IConvoDeleter convoDeleter;
        private readonly IDetermineCurrentEnvironment determineCurrentEnvironment;
        private readonly IHoldAnAccountId accountIdHolder;

        public DeleteDevDataByAccountIdController(
            ILogger<DeleteDevDataByAccountIdController> logger,
            IAccountDeleter accountDeleter,
            IDashDeleter dashDeleter,
            IConvoDeleter convoDeleter,
            IDetermineCurrentEnvironment determineCurrentEnvironment, 
            IHoldAnAccountId accountIdHolder
        )
        {
            this.logger = logger;
            this.accountDeleter = accountDeleter;
            this.dashDeleter = dashDeleter;
            this.convoDeleter = convoDeleter;
            this.determineCurrentEnvironment = determineCurrentEnvironment;
            this.accountIdHolder = accountIdHolder;
        }

        [HttpDelete("dev/{devKey}/{accountId}")]
        public async Task Delete(string accountId, string devKey, CancellationToken cancellationToken)
        {
            if (determineCurrentEnvironment.IsProduction())
            {
                throw new DomainException("Deleting any data is not allowed in production");
            }

            if (devKey != "secretTobyface")
            {
                logger.LogDebug("This is an attempt to Refresh database data.");
                return;
            }

            logger.LogInformation($"Deleting details for account: {accountId}");

            logger.LogInformation("Deleting from the convo database...");
            convoDeleter.DeleteAccount();

            logger.LogInformation("Deleting from the dash database...");
            await dashDeleter.DeleteAccount();

            logger.LogDebug("Deleting from the Accounts database...");
            await accountDeleter.DeleteAccount();

            await accountDeleter.CommitChangesAsync();
            await dashDeleter.CommitChangesAsync();
            await convoDeleter.CommitChangesAsync();
        }
    }
}