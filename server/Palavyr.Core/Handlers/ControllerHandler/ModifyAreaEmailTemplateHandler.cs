using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyAreaEmailTemplateHandler : IRequestHandler<ModifyAreaEmailTemplateRequest, ModifyAreaEmailTemplateResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public ModifyAreaEmailTemplateHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<ModifyAreaEmailTemplateResponse> Handle(ModifyAreaEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var currentArea = await configurationRepository.GetAreaById(request.IntentId);
            currentArea.EmailTemplate = request.EmailTemplate;
            await configurationRepository.CommitChangesAsync();
            return new ModifyAreaEmailTemplateResponse(currentArea.EmailTemplate);
        }
    }

    public class ModifyAreaEmailTemplateResponse
    {
        public ModifyAreaEmailTemplateResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyAreaEmailTemplateRequest : IRequest<ModifyAreaEmailTemplateResponse>
    {
        public string EmailTemplate { get; set; }
        public string IntentId { get; set; }
    }
}