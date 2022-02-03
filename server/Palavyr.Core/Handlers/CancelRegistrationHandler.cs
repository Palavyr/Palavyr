using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories.Delete;

namespace Palavyr.Core.Handlers
{
    public class CancelRegistrationHandler : INotificationHandler<CancelRegistrationNotification>
    {
        private readonly IAccountDeleter accountDeleter;

        public CancelRegistrationHandler(IAccountDeleter accountDeleter)
        {
            this.accountDeleter = accountDeleter;
        }

        public async Task Handle(CancelRegistrationNotification notification, CancellationToken cancellationToken)
        {
            await accountDeleter.DeleteAccount();
            await accountDeleter.CommitChangesAsync();

        }
    }


    public class CancelRegistrationNotification : INotification
    {
        public string EmailAddress { get; set; }
    }
}