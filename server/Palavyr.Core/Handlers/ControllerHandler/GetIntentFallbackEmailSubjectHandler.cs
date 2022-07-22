using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetIntentFallbackEmailSubjectHandler : IRequestHandler<GetIntentFallbackEmailSubjectRequest, GetIntentFallbackEmailSubjectResponse>
    {
        private readonly IEntityStore<Intent> intentStore;

        public GetIntentFallbackEmailSubjectHandler(IEntityStore<Intent> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<GetIntentFallbackEmailSubjectResponse> Handle(GetIntentFallbackEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.Get(request.IntentId, s => s.IntentId);
            var subject = intent.FallbackSubject;
            return new GetIntentFallbackEmailSubjectResponse(subject);
        }
    }

    public class GetIntentFallbackEmailSubjectResponse
    {
        public GetIntentFallbackEmailSubjectResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetIntentFallbackEmailSubjectRequest : IRequest<GetIntentFallbackEmailSubjectResponse>
    {
        public GetIntentFallbackEmailSubjectRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}