using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAreaFallbackEmailSubjectHandler : IRequestHandler<GetAreaFallbackEmailSubjectRequest, GetAreaFallbackEmailSubjectResponse>
    {
        private readonly IEntityStore<Area> intentStore;

        public GetAreaFallbackEmailSubjectHandler(IEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<GetAreaFallbackEmailSubjectResponse> Handle(GetAreaFallbackEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var area = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            var subject = area.FallbackSubject;
            return new GetAreaFallbackEmailSubjectResponse(subject);
        }
    }

    public class GetAreaFallbackEmailSubjectResponse
    {
        public GetAreaFallbackEmailSubjectResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetAreaFallbackEmailSubjectRequest : IRequest<GetAreaFallbackEmailSubjectResponse>
    {
        public GetAreaFallbackEmailSubjectRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}