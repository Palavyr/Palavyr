using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories.Delete;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CancelRegistrationHandler : INotificationHandler<CancelRegistrationNotification>
    {
        private readonly IAccountDeleter accountDeleter;
        private readonly IDashDeleter dashDeleter;
        private readonly IConvoDeleter convoDeleter;

        public CancelRegistrationHandler(
            IAccountDeleter accountDeleter,
            IDashDeleter dashDeleter,
            IConvoDeleter convoDeleter
        )
        {
            this.accountDeleter = accountDeleter;
            this.dashDeleter = dashDeleter;
            this.convoDeleter = convoDeleter;
        }

        public async Task Handle(CancelRegistrationNotification notification, CancellationToken cancellationToken)
        {
            await accountDeleter.DeleteAccount();
            await dashDeleter.DeleteAccount();
            convoDeleter.DeleteAccount();

            await accountDeleter.CommitChangesAsync();
            await dashDeleter.CommitChangesAsync();
            await convoDeleter.CommitChangesAsync();
        }
    }


    public class CancelRegistrationNotification : INotification
    {
        public string EmailAddress { get; set; }
    }
}