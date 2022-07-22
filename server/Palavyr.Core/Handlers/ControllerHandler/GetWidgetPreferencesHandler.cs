using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetWidgetPreferencesHandler : IRequestHandler<GetWidgetPreferencesRequest, GetWidgetPreferencesResponse>
    {
        private readonly IEntityStore<WidgetPreference> widgetPreferenceStore;
        private readonly IMapToNew<WidgetPreference, WidgetPreferencesResource> mapper;

        public GetWidgetPreferencesHandler(IEntityStore<WidgetPreference> widgetPreferenceStore, IMapToNew<WidgetPreference, WidgetPreferencesResource> mapper)
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
        public GetWidgetPreferencesResponse(WidgetPreferencesResource response) => Response = response;
        public WidgetPreferencesResource Response { get; set; }
    }

    public class GetWidgetPreferencesRequest : IRequest<GetWidgetPreferencesResponse>
    {
    }
}