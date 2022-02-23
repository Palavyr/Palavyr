using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyUseAreaFallbackEmailHandler : IRequestHandler<ModifyUseAreaFallbackEmailRequest, ModifyUseAreaFallbackEmailResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public ModifyUseAreaFallbackEmailHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<ModifyUseAreaFallbackEmailResponse> Handle(ModifyUseAreaFallbackEmailRequest request, CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(request.IntentId);
            area.UseAreaFallbackEmail = request.UseFallback;
            await configurationRepository.CommitChangesAsync();
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