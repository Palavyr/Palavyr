using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models;
using Palavyr.Core.Resources;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetWidgetPreCheckHandler : IRequestHandler<GetWidgetPreCheckRequest, GetWidgetPreCheckResponse>
    {
        private readonly IEntityStore<WidgetPreference> widgetStore;
        private readonly IWidgetStatusChecker widgetStatusChecker;
        private readonly ILogger<GetWidgetPreCheckHandler> logger;
        private readonly IEntityStore<Intent> intentStore;
        private readonly IAccountIdTransport accountIdTransport;

        public GetWidgetPreCheckHandler(
            IEntityStore<WidgetPreference> widgetStore,
            IWidgetStatusChecker widgetStatusChecker,
            ILogger<GetWidgetPreCheckHandler> logger,
            IEntityStore<Intent> intentStore,
            IAccountIdTransport accountIdTransport)
        {
            this.widgetStore = widgetStore;
            this.widgetStatusChecker = widgetStatusChecker;
            this.logger = logger;
            this.intentStore = intentStore;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<GetWidgetPreCheckResponse> Handle(GetWidgetPreCheckRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug($"Was the demo query param found? {request.Demo}");
            logger.LogDebug("Running live widget pre-check...");
            logger.LogDebug("Checking if account ID exists...");
            if (accountIdTransport.AccountId == null)
            {
                return new GetWidgetPreCheckResponse(PreCheckResult.CreateApiKeyResult(false));
            }

            var widgetPrefs = await widgetStore.Get(widgetStore.AccountId, s => s.AccountId);
            var areas = await intentStore.GetActiveIntentsWithConvoAndDynamicAndStaticTables();

            var result = await widgetStatusChecker.ExecuteWidgetStatusCheck(areas, widgetPrefs, request.Demo, logger);
            logger.LogDebug($"Pre-check run successful.");
            logger.LogDebug($"Ready result:{result.IsReady}");
            logger.LogDebug($"Incomplete areas: {result.PreCheckErrors.Select(x => x.AreaName).ToList()} ");
            return new GetWidgetPreCheckResponse(result);
        }
    }

    public class GetWidgetPreCheckResponse
    {
        public GetWidgetPreCheckResponse(PreCheckResult response) => Response = response;
        public PreCheckResult Response { get; set; }
    }

    public class GetWidgetPreCheckRequest : IRequest<GetWidgetPreCheckResponse>
    {
        public bool Demo { get; set; }
    }
}