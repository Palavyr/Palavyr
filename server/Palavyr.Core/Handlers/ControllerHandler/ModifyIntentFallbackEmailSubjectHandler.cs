using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyIntentFallbackEmailSubjectHandler : IRequestHandler<ModifyIntentFallbackEmailSubjectRequest, ModifyIntentFallbackEmailSubjectResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public ModifyIntentFallbackEmailSubjectHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyIntentFallbackEmailSubjectResponse> Handle(ModifyIntentFallbackEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var curArea = await intentStore.Get(request.IntentId, s => s.IntentId);

            if (request.Subject != curArea.FallbackSubject)
            {
                curArea.FallbackSubject = request.Subject;
            }

            return new ModifyIntentFallbackEmailSubjectResponse(curArea.FallbackSubject);
        }
    }

    public class ModifyIntentFallbackEmailSubjectResponse
    {
        public ModifyIntentFallbackEmailSubjectResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyIntentFallbackEmailSubjectRequest : IRequest<ModifyIntentFallbackEmailSubjectResponse>
    {
        public string Subject { get; set; }
        public string IntentId { get; set; }
    }
}