using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetWidgetPrecheckHandler : IRequestHandler<GetWidgetPrecheckRequest, GetWidgetPrecheckResponse>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IWidgetStatusChecker widgetStatusChecker;
        private readonly ILogger<GetWidgetPrecheckHandler> logger;
        private readonly IHoldAnAccountId accountId;

        public GetWidgetPrecheckHandler(
            IConfigurationRepository configurationRepository,
            IWidgetStatusChecker widgetStatusChecker,
            ILogger<GetWidgetPrecheckHandler> logger,
            IHoldAnAccountId accountId)
        {
            this.configurationRepository = configurationRepository;
            this.widgetStatusChecker = widgetStatusChecker;
            this.logger = logger;
            this.accountId = accountId;
        }

        public async Task<GetWidgetPrecheckResponse> Handle(GetWidgetPrecheckRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug($"Was the demo query param found? {request.Demo}");
            logger.LogDebug("Running live widget pre-check...");
            logger.LogDebug("Checking if account ID exists...");
            if (accountId.AccountId == null)
            {
                return new GetWidgetPrecheckResponse(PreCheckResult.CreateApiKeyResult(false));
            }

            var widgetPrefs = await configurationRepository.GetWidgetPreferences();
            var areas = await configurationRepository.GetActiveAreasWithConvoAndDynamicAndStaticTables();

            var result = await widgetStatusChecker.ExecuteWidgetStatusCheck(areas, widgetPrefs, request.Demo, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.PreCheckErrors.Select(x => x.AreaName).ToList()} ");
            return new GetWidgetPrecheckResponse(result);
        }
    }

    public class GetWidgetPrecheckResponse
    {
        public GetWidgetPrecheckResponse(PreCheckResult response) => Response = response;
        public PreCheckResult Response { get; set; }
    }

    public class GetWidgetPrecheckRequest : IRequest<GetWidgetPrecheckResponse>
    {
        public bool Demo { get; set; }
    }
}