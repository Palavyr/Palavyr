using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Repositories.Delete;

namespace Palavyr.API.Controllers.Accounts
{
    public class DeleteAccountHandler: IRequestHandler<DeleteAccountRequest>
    {
        private readonly IAccountDeleter accountDeleter;
        private readonly IDashDeleter dashDeleter;
        private readonly IConvoDeleter convoDeleter;
        private readonly ILogger<DeleteAccountHandler> logger;

        public DeleteAccountHandler(
            IAccountDeleter accountDeleter,
            IDashDeleter dashDeleter,
            IConvoDeleter convoDeleter,
            ILogger<DeleteAccountHandler> logger
            )
        {
            this.accountDeleter = accountDeleter;
            this.dashDeleter = dashDeleter;
            this.convoDeleter = convoDeleter;
            this.logger = logger;
        }
        public async Task<Unit> Handle(DeleteAccountRequest request, CancellationToken cancellationToken)
        {
            var accountId = request.AccountId;
            if (accountId == null)
            {
                throw new DomainException("Please log out and log in again before trying to delete your account");
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
            return default;
        }
    }

    public class DeleteAccountRequest : IRequest<Unit>
    {
        public string AccountId { get; set; }
    }
}