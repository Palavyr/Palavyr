using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAreaEmailSubjectHandler : IRequestHandler<GetAreaEmailSubjectRequest, GetAreaEmailSubjectResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public GetAreaEmailSubjectHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<GetAreaEmailSubjectResponse> Handle(GetAreaEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(request.IntentId);
            var subject = area.Subject;
            return new GetAreaEmailSubjectResponse(subject);
        }
    }

    public class GetAreaEmailSubjectResponse
    {
        public GetAreaEmailSubjectResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetAreaEmailSubjectRequest : IRequest<GetAreaEmailSubjectResponse>
    {
        public GetAreaEmailSubjectRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}