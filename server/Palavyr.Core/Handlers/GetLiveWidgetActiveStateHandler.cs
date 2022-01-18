using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class GetLiveWidgetActiveStateHandler : IRequestHandler<GetLiveWidgetActiveStateRequest, GetLiveWidgetActiveStateResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public GetLiveWidgetActiveStateHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<GetLiveWidgetActiveStateResponse> Handle(GetLiveWidgetActiveStateRequest request, CancellationToken cancellationToken)
        {
            var widgetPreferences = await configurationRepository.GetWidgetPreferences();
            return new GetLiveWidgetActiveStateResponse(widgetPreferences.WidgetState);
        }
    }

    public class GetLiveWidgetActiveStateResponse
    {
        public GetLiveWidgetActiveStateResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class GetLiveWidgetActiveStateRequest : IRequest<GetLiveWidgetActiveStateResponse>
    {
    }
}