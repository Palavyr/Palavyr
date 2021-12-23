using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Sessions;

namespace Palavyr.API.CustomMiddleware
{
    public class SetCancellationTokenHandler : INotificationHandler<SetCancellationTokenRequest>
    {
        private readonly ITransportACancellationToken cancellationTokenTransport;

        public SetCancellationTokenHandler(ITransportACancellationToken cancellationTokenTransport)
        {
            this.cancellationTokenTransport = cancellationTokenTransport;
        }

        public async Task Handle(SetCancellationTokenRequest notification, CancellationToken cancellationToken)
        {
            cancellationTokenTransport.Assign(notification.CancellationToken);
            await Task.CompletedTask;
        }
    }

    public class SetCancellationTokenRequest : INotification
    {
        public CancellationToken CancellationToken { get; set; }
        public SetCancellationTokenRequest(CancellationToken contextRequestAborted) => CancellationToken = contextRequestAborted;
    }
}