using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyAreaFallbackEmailTemplateHandler : IRequestHandler<ModifyAreaFallbackEmailTemplateRequest, ModifyAreaFallbackEmailTemplateResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public ModifyAreaFallbackEmailTemplateHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<ModifyAreaFallbackEmailTemplateResponse> Handle(ModifyAreaFallbackEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var currentArea = await configurationRepository.GetAreaById(request.IntentId);
            currentArea.FallbackEmailTemplate = request.EmailTemplate;
            await configurationRepository.CommitChangesAsync();
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