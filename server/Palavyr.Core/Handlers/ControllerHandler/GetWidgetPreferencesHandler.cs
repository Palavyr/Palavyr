using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetWidgetPreferencesHandler : IRequestHandler<GetWidgetPreferencesRequest, GetWidgetPreferencesResponse>
    {
        private readonly IConfigurationRepository configurationRepository;

        public GetWidgetPreferencesHandler(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task<GetWidgetPreferencesResponse> Handle(GetWidgetPreferencesRequest request, CancellationToken cancellationToken)
        {
            var prefs = await configurationRepository.GetWidgetPreferences();
            return new GetWidgetPreferencesResponse(prefs);
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