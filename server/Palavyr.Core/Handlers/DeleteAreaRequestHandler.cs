using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.Deletion;

namespace Palavyr.Core.Handlers
{
    public class DeleteAreaRequestHandler : INotificationHandler<DeleteAreaRequest>
    {
        private readonly IAreaDeleter areaDeleter;

        public DeleteAreaRequestHandler(IAreaDeleter areaDeleter)
        {
            this.areaDeleter = areaDeleter;
        }

        public async Task Handle(DeleteAreaRequest request, CancellationToken cancellationToken)
        {
            await areaDeleter.DeleteArea(request.IntentId, cancellationToken);
        }
    }

    public class DeleteAreaRequest : INotification
    {
        public string IntentId { get; set; }
    }
}