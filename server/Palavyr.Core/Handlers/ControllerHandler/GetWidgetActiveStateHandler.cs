using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetWidgetActiveStateHandler : IRequestHandler<GetWidgetActiveStateRequest, GetWidgetActiveStateResponse>
    {
        private readonly ILogger<GetWidgetActiveStateHandler> logger;
        private readonly IEntityStore<WidgetPreference> widgetPreferenceStore;

        public GetWidgetActiveStateHandler(ILogger<GetWidgetActiveStateHandler> logger, IEntityStore<WidgetPreference> widgetPreferenceStore)
        {
            this.logger = logger;
            this.widgetPreferenceStore = widgetPreferenceStore;
        }

        public async Task<GetWidgetActiveStateResponse> Handle(GetWidgetActiveStateRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Retrieving widget state.");
            var widgetPreferences = await widgetPreferenceStore.Get(widgetPreferenceStore.AccountId, s => s.AccountId);
            var activeState = widgetPreferences.WidgetState;
            
            return new GetWidgetActiveStateResponse(activeState);

        }
    }
    
    public class GetWidgetActiveStateRequest : IRequest<GetWidgetActiveStateResponse>
    {
        public GetWidgetActiveStateRequest()
        {
        }
    }

    public class GetWidgetActiveStateResponse
    {
        public bool Response { get; }

        public GetWidgetActiveStateResponse(bool response) => Response = response;
    }

}