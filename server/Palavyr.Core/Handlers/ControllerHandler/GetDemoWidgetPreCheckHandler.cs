using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetDemoWidgetPreCheckHandler : IRequestHandler<GetDemoWidgetPreCheckRequest, GetDemoWidgetPreCheckResponse>
    {
        private readonly ILogger<GetDemoWidgetPreCheckHandler> logger;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IWidgetStatusChecker widgetStatusChecker;

        public GetDemoWidgetPreCheckHandler(
            ILogger<GetDemoWidgetPreCheckHandler> logger,
            IConfigurationRepository configurationRepository,
            IWidgetStatusChecker widgetStatusChecker)
        {
            this.logger = logger;
            this.configurationRepository = configurationRepository;
            this.widgetStatusChecker = widgetStatusChecker;
        }

        public async Task<GetDemoWidgetPreCheckResponse> Handle(GetDemoWidgetPreCheckRequest request, CancellationToken cancellationToken)
        {
            var widgetPrefs = await configurationRepository.GetWidgetPreferences();
            var areas = await configurationRepository.GetActiveAreasWithConvoAndDynamicAndStaticTables();

            var result = await widgetStatusChecker.ExecuteWidgetStatusCheck(areas, widgetPrefs, true, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.PreCheckErrors.Select(x => x.AreaName).ToList()}");
            return new GetDemoWidgetPreCheckResponse(result);
        }
    }

    public class GetDemoWidgetPreCheckResponse
    {
        public GetDemoWidgetPreCheckResponse(PreCheckResult response) => Response = response;
        public PreCheckResult Response { get; set; }
    }

    public class GetDemoWidgetPreCheckRequest : IRequest<GetDemoWidgetPreCheckResponse>
    {
    }
}