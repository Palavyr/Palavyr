using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Stores.Delete;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class DeleteDevDataByAccountIdHandler : INotificationHandler<DeleteDevDataByAccountIdRequest>
    {
        private readonly ILogger<DeleteDevDataByAccountIdHandler> logger;
        private readonly IDangerousAccountDeleter dangerousAccountDeleter;
        private readonly IDetermineCurrentEnvironment determineCurrentEnvironment;

        public DeleteDevDataByAccountIdHandler(
            ILogger<DeleteDevDataByAccountIdHandler> logger,
            IDangerousAccountDeleter dangerousAccountDeleter,
            IDetermineCurrentEnvironment determineCurrentEnvironment)
        {
            this.logger = logger;
            this.dangerousAccountDeleter = dangerousAccountDeleter;
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

            await dangerousAccountDeleter.DeleteAllThings();
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