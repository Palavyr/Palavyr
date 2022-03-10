using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Stores.Delete;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CancelRegistrationHandler : INotificationHandler<CancelRegistrationNotification>
    {
        private readonly IDangerousAccountDeleter dangerousAccountDeleter;


        public CancelRegistrationHandler(
            IDangerousAccountDeleter dangerousAccountDeleter)
        {
            this.dangerousAccountDeleter = dangerousAccountDeleter;
        }

        public async Task Handle(CancelRegistrationNotification notification, CancellationToken cancellationToken)
        {
            await dangerousAccountDeleter.DeleteAllThings();
        }
    }


    public class CancelRegistrationNotification : INotification
    {
        public string EmailAddress { get; set; }
    }
}