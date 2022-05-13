using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetWidgetPreferencesHandler : IRequestHandler<GetWidgetPreferencesRequest, GetWidgetPreferencesResponse>
    {
        private readonly IEntityStore<WidgetPreference> widgetPreferenceStore;
        private readonly IMapToNew<WidgetPreference, WidgetPreferenceResource> mapper;

        public GetWidgetPreferencesHandler(IEntityStore<WidgetPreference> widgetPreferenceStore, IMapToNew<WidgetPreference, WidgetPreferenceResource> mapper)
        {
            this.widgetPreferenceStore = widgetPreferenceStore;
            this.mapper = mapper;
        }

        public async Task<GetWidgetPreferencesResponse> Handle(GetWidgetPreferencesRequest request, CancellationToken cancellationToken)
        {
            var widgetPreferences = await widgetPreferenceStore.Get(widgetPreferenceStore.AccountId, s => s.AccountId);
            var resource = await mapper.Map(widgetPreferences);
            return new GetWidgetPreferencesResponse(resource);
        }
    }

    public class GetWidgetPreferencesResponse
    {
        public GetWidgetPreferencesResponse(WidgetPreferenceResource response) => Response = response;
        public WidgetPreferenceResource Response { get; set; }
    }

    public class GetWidgetPreferencesRequest : IRequest<GetWidgetPreferencesResponse>
    {
    }
}