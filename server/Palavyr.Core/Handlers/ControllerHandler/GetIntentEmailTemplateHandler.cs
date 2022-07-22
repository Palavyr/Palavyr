using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetIntentEmailTemplateHandler : IRequestHandler<GetIntentEmailTemplateRequest, GetIntentEmailTemplateResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public GetIntentEmailTemplateHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<GetIntentEmailTemplateResponse> Handle(GetIntentEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.Get(request.IntentId, s => s.IntentId);
            var emailTemplate = intent.EmailTemplate;
            return new GetIntentEmailTemplateResponse(emailTemplate);
        }
    }

    public class GetIntentEmailTemplateResponse
    {
        public GetIntentEmailTemplateResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetIntentEmailTemplateRequest : IRequest<GetIntentEmailTemplateResponse>
    {
        public GetIntentEmailTemplateRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}