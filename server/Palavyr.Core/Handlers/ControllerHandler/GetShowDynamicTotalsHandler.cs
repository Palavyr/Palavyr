using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetShowDynamicTotalsHandlerHandler : IRequestHandler<GetShowDynamicTotalsHandlerRequest, GetShowDynamicTotalsHandlerResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public GetShowDynamicTotalsHandlerHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<GetShowDynamicTotalsHandlerResponse> Handle(GetShowDynamicTotalsHandlerRequest request, CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(request.IntentId);
            return new GetShowDynamicTotalsHandlerResponse(area.IncludeDynamicTableTotals);
        }
    }

    public class GetShowDynamicTotalsHandlerResponse
    {
        public GetShowDynamicTotalsHandlerResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class GetShowDynamicTotalsHandlerRequest : IRequest<GetShowDynamicTotalsHandlerResponse>
    {
        public string IntentId { get; set; }

        public GetShowDynamicTotalsHandlerRequest(string intentId)
        {
            IntentId = intentId;
        }
    }
}