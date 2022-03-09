using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyWidgetActiveStateHandler : IRequestHandler<ModifyWidgetActiveStateRequest, ModifyWidgetActiveStateResponse>
    {
        private readonly IConfigurationEntityStore<WidgetPreference> widgetPreferenceStore;
        private readonly ILogger<ModifyWidgetActiveStateHandler> logger;

        public ModifyWidgetActiveStateHandler(IConfigurationEntityStore<WidgetPreference> widgetPreferenceStore, ILogger<ModifyWidgetActiveStateHandler> logger)
        {
            this.widgetPreferenceStore = widgetPreferenceStore;
            this.logger = logger;
        }

        public async Task<ModifyWidgetActiveStateResponse> Handle(ModifyWidgetActiveStateRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Modifying widget preference");
            var widgetPreferences = await widgetPreferenceStore.Get(widgetPreferenceStore.AccountId, s => s.AccountId);
            widgetPreferences.WidgetState = request.State;
            return new ModifyWidgetActiveStateResponse(widgetPreferences.WidgetState);
        }
    }

    public class ModifyWidgetActiveStateResponse
    {
        public ModifyWidgetActiveStateResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ModifyWidgetActiveStateRequest : IRequest<ModifyWidgetActiveStateResponse>
    {
        public ModifyWidgetActiveStateRequest(bool state)
        {
            State = state;
        }

        public bool State { get; set; }
    }
}