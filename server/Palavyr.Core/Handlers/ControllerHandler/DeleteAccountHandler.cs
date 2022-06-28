using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores.Delete;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountRequest>
    {
        private readonly IDangerousAccountDeleter dangerousAccountDeleter;
        private readonly ILogger<DeleteAccountHandler> logger;
        private readonly IAccountIdTransport accountIdTransport;

        public DeleteAccountHandler(
            IDangerousAccountDeleter dangerousAccountDeleter,
            ILogger<DeleteAccountHandler> logger,
            IAccountIdTransport accountIdTransport
        )
        {
            this.dangerousAccountDeleter = dangerousAccountDeleter;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<Unit> Handle(DeleteAccountRequest request, CancellationToken _)
        {
            logger.LogInformation("Deleting details for account: {Account}", accountIdTransport.AccountId);
            await dangerousAccountDeleter.DeleteAllThings();
            return default;
        }
    }

    public class DeleteAccountRequest : IRequest<object>, IRequest<Unit>
    {
        public const string Route = "account/delete-account";
    }
}