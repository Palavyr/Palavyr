using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Repositories.Delete;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class DeleteDevDataByAccountIdHandler : INotificationHandler<DeleteDevDataByAccountIdRequest>
    {
        private readonly ILogger<DeleteDevDataByAccountIdHandler> logger;
        private readonly IAccountDeleter accountDeleter;
        private readonly IDashDeleter dashDeleter;
        private readonly IConvoDeleter convoDeleter;
        private readonly IDetermineCurrentEnvironment determineCurrentEnvironment;

        public DeleteDevDataByAccountIdHandler(
            ILogger<DeleteDevDataByAccountIdHandler> logger,
            IAccountDeleter accountDeleter,
            IDashDeleter dashDeleter,
            IConvoDeleter convoDeleter,
            IDetermineCurrentEnvironment determineCurrentEnvironment)
        {
            this.logger = logger;
            this.accountDeleter = accountDeleter;
            this.dashDeleter = dashDeleter;
            this.convoDeleter = convoDeleter;
            this.determineCurrentEnvironment = determineCurrentEnvironment;
        }

        public async Task Handle(DeleteDevDataByAccountIdRequest request, CancellationToken cancellationToken)
        {
            if (determineCurrentEnvironment.IsProduction())
            {
                throw new DomainException("Deleting any data is not allowed in production");
            }

            if (request.DevKey != "secretTobyface")
            {
                logger.LogDebug("This is an attempt to Refresh database data.");
                return;
            }

            logger.LogInformation($"Deleting details for account: {request.AccountId}");

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


    public class DeleteDevDataByAccountIdRequest : INotification
    {
        public DeleteDevDataByAccountIdRequest(string accountId, string devKey)
        {
            AccountId = accountId;
            DevKey = devKey;
        }

        public string DevKey { get; set; }
        public string AccountId { get; set; }
    }
}