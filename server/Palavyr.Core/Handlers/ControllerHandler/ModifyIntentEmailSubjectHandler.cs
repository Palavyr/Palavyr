using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyIntentEmailSubjectHandler : IRequestHandler<ModifyIntentEmailSubjectRequest, ModifyIntentEmailSubjectResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public ModifyIntentEmailSubjectHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyIntentEmailSubjectResponse> Handle(ModifyIntentEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var currentIntent = await intentStore.Get(request.IntentId, s => s.IntentId);
            if (request.Subject != currentIntent.Subject)
            {
                currentIntent.Subject = request.Subject;
            }

            return new ModifyIntentEmailSubjectResponse(request.Subject);
        }
    }

    public class ModifyIntentEmailSubjectResponse
    {
        public ModifyIntentEmailSubjectResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyIntentEmailSubjectRequest : IRequest<ModifyIntentEmailSubjectResponse>
    {
        public const string Route = "email/subject";

        public string Subject { get; set; }
        public string IntentId { get; set; }
    }
}