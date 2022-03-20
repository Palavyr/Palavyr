using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyUseAreaFallbackEmailHandler : IRequestHandler<ModifyUseAreaFallbackEmailRequest, ModifyUseAreaFallbackEmailResponse>
    {
        private readonly IEntityStore<Area> intentStore;

        public ModifyUseAreaFallbackEmailHandler(IEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyUseAreaFallbackEmailResponse> Handle(ModifyUseAreaFallbackEmailRequest request, CancellationToken cancellationToken)
        {
            var area = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            area.UseAreaFallbackEmail = request.UseFallback;
            return new ModifyUseAreaFallbackEmailResponse(area.UseAreaFallbackEmail);
        }
    }

    public class ModifyUseAreaFallbackEmailResponse
    {
        public ModifyUseAreaFallbackEmailResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ModifyUseAreaFallbackEmailRequest : IRequest<ModifyUseAreaFallbackEmailResponse>
    {
        public string IntentId { get; set; }
        public bool UseFallback { get; set; }
    }
}