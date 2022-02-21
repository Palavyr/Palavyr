using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAreaFallbackEmailSubjectHandler : IRequestHandler<GetAreaFallbackEmailSubjectRequest, GetAreaFallbackEmailSubjectResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public GetAreaFallbackEmailSubjectHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<GetAreaFallbackEmailSubjectResponse> Handle(GetAreaFallbackEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(request.IntentId);
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