using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyIntentEmailTemplateHandler : IRequestHandler<ModifyIntentEmailTemplateRequest, ModifyIntentEmailTemplateResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public ModifyIntentEmailTemplateHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyIntentEmailTemplateResponse> Handle(ModifyIntentEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var currentArea = await intentStore.Get(request.IntentId, s => s.IntentId);
            currentArea.EmailTemplate = request.EmailTemplate;

            return new ModifyIntentEmailTemplateResponse(currentArea.EmailTemplate);
        }
    }

    public class ModifyIntentEmailTemplateResponse
    {
        public ModifyIntentEmailTemplateResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyIntentEmailTemplateRequest : IRequest<ModifyIntentEmailTemplateResponse>
    {
        public const string Route = "email/email-template";

        public string EmailTemplate { get; set; }
        public string IntentId { get; set; }
    }
}