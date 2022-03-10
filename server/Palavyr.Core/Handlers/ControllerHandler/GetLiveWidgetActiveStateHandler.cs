using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetLiveWidgetActiveStateHandler : IRequestHandler<GetLiveWidgetActiveStateRequest, GetLiveWidgetActiveStateResponse>
    {
        private readonly IEntityStore<WidgetPreference> widgetPreferenceStore;

        public GetLiveWidgetActiveStateHandler(IEntityStore<WidgetPreference> widgetPreferenceStore)
        {
            this.widgetPreferenceStore = widgetPreferenceStore;
        }

        public async Task<GetLiveWidgetActiveStateResponse> Handle(GetLiveWidgetActiveStateRequest request, CancellationToken cancellationToken)
        {
            var widgetPreferences = await widgetPreferenceStore.Get(widgetPreferenceStore.AccountId, s => s.AccountId);
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