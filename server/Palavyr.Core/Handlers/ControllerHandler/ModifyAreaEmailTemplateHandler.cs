using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyAreaEmailTemplateHandler : IRequestHandler<ModifyAreaEmailTemplateRequest, ModifyAreaEmailTemplateResponse>
    {
        private readonly IEntityStore<Area> intentStore;

        public ModifyAreaEmailTemplateHandler(IEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyAreaEmailTemplateResponse> Handle(ModifyAreaEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var currentArea = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            currentArea.EmailTemplate = request.EmailTemplate;

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
        public const string Route = "email/email-template";

        public string EmailTemplate { get; set; }
        public string IntentId { get; set; }
    }
}