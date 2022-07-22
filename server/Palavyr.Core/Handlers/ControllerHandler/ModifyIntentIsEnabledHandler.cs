using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyIntentIsEnabledHandler : IRequestHandler<ModifyIntentIsCompleteRequest, ModifyIntentIsCompleteResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public ModifyIntentIsEnabledHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyIntentIsCompleteResponse> Handle(ModifyIntentIsCompleteRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.Get(request.IntentId, s => s.IntentId);
            intent.IsEnabled = request.IsEnabled;
            return new ModifyIntentIsCompleteResponse(intent.IsEnabled);
        }
    }

    public class ModifyIntentIsCompleteResponse
    {
        public ModifyIntentIsCompleteResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ModifyIntentIsCompleteRequest : IRequest<ModifyIntentIsCompleteResponse>
    {
        public string IntentId { get; set; }
        public bool IsEnabled { get; set; }
    }
}