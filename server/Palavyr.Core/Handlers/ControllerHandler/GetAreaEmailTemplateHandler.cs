using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAreaEmailTemplateHandler : IRequestHandler<GetAreaEmailTemplateRequest, GetAreaEmailTemplateResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public GetAreaEmailTemplateHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<GetAreaEmailTemplateResponse> Handle(GetAreaEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(request.IntentId);
            var emailTemplate = area.EmailTemplate;
            return new GetAreaEmailTemplateResponse(emailTemplate);
        }
    }

    public class GetAreaEmailTemplateResponse
    {
        public GetAreaEmailTemplateResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetAreaEmailTemplateRequest : IRequest<GetAreaEmailTemplateResponse>
    {
        public GetAreaEmailTemplateRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}