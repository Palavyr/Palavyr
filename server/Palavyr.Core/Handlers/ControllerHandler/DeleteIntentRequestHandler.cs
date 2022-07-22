using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.Deletion;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class DeleteIntentRequestHandler : INotificationHandler<DeleteIntentRequest>
    {
        private readonly IIntentDeleter intentDeleter;

        public DeleteIntentRequestHandler(IIntentDeleter intentDeleter)
        {
            this.intentDeleter = intentDeleter;
        }

        public async Task Handle(DeleteIntentRequest request, CancellationToken cancellationToken)
        {
            await intentDeleter.DeleteIntent(request.IntentId, cancellationToken);
        }
    }

    public class DeleteIntentRequest : INotification
    {
        public string IntentId { get; set; }
    }
}