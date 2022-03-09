using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAreaEmailTemplateHandler : IRequestHandler<GetAreaEmailTemplateRequest, GetAreaEmailTemplateResponse>
    {
        private readonly IConfigurationEntityStore<Area> intentStore;

        public GetAreaEmailTemplateHandler(IConfigurationEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<GetAreaEmailTemplateResponse> Handle(GetAreaEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var area = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
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