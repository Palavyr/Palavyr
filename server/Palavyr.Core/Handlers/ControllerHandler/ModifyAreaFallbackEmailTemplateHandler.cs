using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyAreaFallbackEmailTemplateHandler : IRequestHandler<ModifyAreaFallbackEmailTemplateRequest, ModifyAreaFallbackEmailTemplateResponse>
    {
        private readonly IEntityStore<Area> intentStore;

        public ModifyAreaFallbackEmailTemplateHandler(IEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyAreaFallbackEmailTemplateResponse> Handle(ModifyAreaFallbackEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var currentArea = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            currentArea.FallbackEmailTemplate = request.EmailTemplate;
            return new ModifyAreaFallbackEmailTemplateResponse(currentArea.FallbackEmailTemplate);
        }
    }

    public class ModifyAreaFallbackEmailTemplateResponse
    {
        public ModifyAreaFallbackEmailTemplateResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyAreaFallbackEmailTemplateRequest : IRequest<ModifyAreaFallbackEmailTemplateResponse>
    {
        public string EmailTemplate { get; set; }
        public string IntentId { get; set; }
    }
}