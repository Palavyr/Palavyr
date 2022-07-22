using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetIntentEmailSubjectHandler : IRequestHandler<GetIntentEmailSubjectRequest, GetIntentEmailSubjectResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public GetIntentEmailSubjectHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<GetIntentEmailSubjectResponse> Handle(GetIntentEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.Get(request.IntentId, s => s.IntentId);
            var subject = intent.Subject;
            return new GetIntentEmailSubjectResponse(subject);
        }
    }

    public class GetIntentEmailSubjectResponse
    {
        public GetIntentEmailSubjectResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetIntentEmailSubjectRequest : IRequest<GetIntentEmailSubjectResponse>
    {
        public GetIntentEmailSubjectRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}