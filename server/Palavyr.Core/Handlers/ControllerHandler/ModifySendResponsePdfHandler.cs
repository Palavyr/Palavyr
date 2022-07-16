using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifySendResponsePdfHandler : IRequestHandler<ModifySendResponseRequest, ModifySendResponseResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public ModifySendResponsePdfHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifySendResponseResponse> Handle(ModifySendResponseRequest request, CancellationToken cancellationToken)
        {
            var area = await intentStore.Get(request.IntentId, s => s.IntentId);
            var newState = !area.SendPdfResponse;
            area.SendPdfResponse = newState;
            return new ModifySendResponseResponse(newState);
        }
    }

    public class ModifySendResponseResponse
    {
        public ModifySendResponseResponse(bool newState)
        {
            NewState = newState;
        }

        public bool NewState { get; set; }
    }

    public class ModifySendResponseRequest : IRequest<ModifySendResponseResponse>
    {
        public ModifySendResponseRequest(string intentId)
        {
            IntentId = intentId;
        }

        public static string FormatRoute(string intentId)
        {
            return Route.Replace("{intentId}", intentId);
        }

        public const string Route = "area/send-pdf/{intentId}";
        public string IntentId { get; set; }
    }
}