using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyUseIntentFallbackEmailHandler : IRequestHandler<ModifyUseIntentFallbackEmailRequest, ModifyUseIntentFallbackEmailResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public ModifyUseIntentFallbackEmailHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyUseIntentFallbackEmailResponse> Handle(ModifyUseIntentFallbackEmailRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.Get(request.IntentId, s => s.IntentId);
            intent.UseIntentFallbackEmail = request.UseFallback;
            return new ModifyUseIntentFallbackEmailResponse(intent.UseIntentFallbackEmail);
        }
    }

    public class ModifyUseIntentFallbackEmailResponse
    {
        public ModifyUseIntentFallbackEmailResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ModifyUseIntentFallbackEmailRequest : IRequest<ModifyUseIntentFallbackEmailResponse>
    {
        public string IntentId { get; set; }
        public bool UseFallback { get; set; }
    }
}