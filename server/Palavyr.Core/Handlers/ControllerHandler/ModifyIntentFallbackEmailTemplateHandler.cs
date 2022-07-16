using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyIntentFallbackEmailTemplateHandler : IRequestHandler<ModifyIntentFallbackEmailTemplateRequest, ModifyIntentFallbackEmailTemplateResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public ModifyIntentFallbackEmailTemplateHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyIntentFallbackEmailTemplateResponse> Handle(ModifyIntentFallbackEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var currentArea = await intentStore.Get(request.IntentId, s => s.IntentId);
            currentArea.FallbackEmailTemplate = request.EmailTemplate;
            return new ModifyIntentFallbackEmailTemplateResponse(currentArea.FallbackEmailTemplate);
        }
    }

    public class ModifyIntentFallbackEmailTemplateResponse
    {
        public ModifyIntentFallbackEmailTemplateResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyIntentFallbackEmailTemplateRequest : IRequest<ModifyIntentFallbackEmailTemplateResponse>
    {
        public string EmailTemplate { get; set; }
        public string IntentId { get; set; }
    }
}