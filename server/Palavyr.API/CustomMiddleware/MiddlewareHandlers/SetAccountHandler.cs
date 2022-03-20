using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Sessions;

namespace Palavyr.API.CustomMiddleware.MiddlewareHandlers
{
    public class SetAccountHandler : INotificationHandler<SetAccountEvent>
    {
        private readonly IAccountIdTransport accountIdTransport;

        public SetAccountHandler(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }

        public async Task Handle(SetAccountEvent notification, CancellationToken cancellationToken)
        {
            accountIdTransport.Assign(notification.SessionAccountId);
            await Task.CompletedTask;
        }
    }

    public class SetAccountEvent : INotification
    {
        public string SessionAccountId { get; }

        public SetAccountEvent(string sessionAccountId)
        {
            SessionAccountId = sessionAccountId;
        }
    }
}