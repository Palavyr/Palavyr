using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAreaFallbackEmailTemplateHandler : IRequestHandler<GetAreaFallbackEmailTemplateRequest, GetAreaFallbackEmailTemplateResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public GetAreaFallbackEmailTemplateHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<GetAreaFallbackEmailTemplateResponse> Handle(GetAreaFallbackEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(request.IntentId);
            var emailTemplate = area.FallbackEmailTemplate;
            return new GetAreaFallbackEmailTemplateResponse(emailTemplate);
        }
    }

    public class GetAreaFallbackEmailTemplateResponse
    {
        public GetAreaFallbackEmailTemplateResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetAreaFallbackEmailTemplateRequest : IRequest<GetAreaFallbackEmailTemplateResponse>
    {
        public GetAreaFallbackEmailTemplateRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}