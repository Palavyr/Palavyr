using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAreaFallbackEmailTemplateHandler : IRequestHandler<GetAreaFallbackEmailTemplateRequest, GetAreaFallbackEmailTemplateResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public GetAreaFallbackEmailTemplateHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<GetAreaFallbackEmailTemplateResponse> Handle(GetAreaFallbackEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            var emailTemplate = intent.FallbackEmailTemplate;
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