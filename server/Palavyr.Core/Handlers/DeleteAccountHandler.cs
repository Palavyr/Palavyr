using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.Delete;

namespace Palavyr.Core.Handlers
{
    public class DeleteAccountHandler: IRequestHandler<DeleteAccountRequest>
    {
        private readonly IAccountDeleter accountDeleter;
        private readonly IDashDeleter dashDeleter;
        private readonly IConvoDeleter convoDeleter;
        private readonly ILogger<DeleteAccountHandler> logger;
        private readonly IAccountRepository accountRepository;

        public DeleteAccountHandler(
            IAccountDeleter accountDeleter,
            IDashDeleter dashDeleter,
            IConvoDeleter convoDeleter,
            ILogger<DeleteAccountHandler> logger,
            IAccountRepository accountRepository
            )
        {
            this.accountDeleter = accountDeleter;
            this.dashDeleter = dashDeleter;
            this.convoDeleter = convoDeleter;
            this.logger = logger;
            this.accountRepository = accountRepository;
        }
        public async Task<Unit> Handle(DeleteAccountRequest request, CancellationToken _)
        {
            logger.LogInformation($"Deleting details for account: {accountRepository.AccountIdHolder.AccountId}");

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
    }
}