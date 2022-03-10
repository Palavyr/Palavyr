using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetWidgetPreferencesHandler : IRequestHandler<GetWidgetPreferencesRequest, GetWidgetPreferencesResponse>
    {
        private readonly IEntityStore<WidgetPreference> widgetPreferenceStore;

        public GetWidgetPreferencesHandler(IEntityStore<WidgetPreference> widgetPreferenceStore)
        {
            this.widgetPreferenceStore = widgetPreferenceStore;
        }

        public async Task<GetWidgetPreferencesResponse> Handle(GetWidgetPreferencesRequest request, CancellationToken cancellationToken)
        {
            var widgetPreferences = await widgetPreferenceStore.Get(widgetPreferenceStore.AccountId, s => s.AccountId);
            return new GetWidgetPreferencesResponse(widgetPreferences);
        }
    }

    public class GetWidgetPreferencesResponse
    {
        public GetWidgetPreferencesResponse(WidgetPreference response) => Response = response;
        public WidgetPreference Response { get; set; }
    }

    public class GetWidgetPreferencesRequest : IRequest<GetWidgetPreferencesResponse>
    {
    }
}