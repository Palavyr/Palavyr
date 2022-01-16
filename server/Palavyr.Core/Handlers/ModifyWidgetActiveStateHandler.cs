using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class ModifyWidgetActiveStateHandler : IRequestHandler<ModifyWidgetActiveStateRequest, ModifyWidgetActiveStateResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly ILogger<ModifyWidgetActiveStateHandler> logger;

        public ModifyWidgetActiveStateHandler(IConfigurationRepository configurationRepository, ILogger<ModifyWidgetActiveStateHandler> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        public async Task<ModifyWidgetActiveStateResponse> Handle(ModifyWidgetActiveStateRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Modifying widget preference");
            var widgetPrefs = await configurationRepository.GetWidgetPreferences();
            widgetPrefs.WidgetState = request.State;
            await configurationRepository.CommitChangesAsync();
            return new ModifyWidgetActiveStateResponse(widgetPrefs.WidgetState);
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