using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetIntentFallbackEmailTemplateHandler : IRequestHandler<GetIntentFallbackEmailTemplateRequest, GetIntentFallbackEmailTemplateResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public GetIntentFallbackEmailTemplateHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<GetIntentFallbackEmailTemplateResponse> Handle(GetIntentFallbackEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.Get(request.IntentId, s => s.IntentId);
            var emailTemplate = intent.FallbackEmailTemplate;
            return new GetIntentFallbackEmailTemplateResponse(emailTemplate);
        }
    }

    public class GetIntentFallbackEmailTemplateResponse
    {
        public GetIntentFallbackEmailTemplateResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetIntentFallbackEmailTemplateRequest : IRequest<GetIntentFallbackEmailTemplateResponse>
    {
        public GetIntentFallbackEmailTemplateRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}