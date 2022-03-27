using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetDemoWidgetPreCheckHandler : IRequestHandler<GetDemoWidgetPreCheckRequest, GetDemoWidgetPreCheckResponse>
    {
        private readonly ILogger<GetDemoWidgetPreCheckHandler> logger;
        private readonly IEntityStore<Area> intentStore;
        private readonly IEntityStore<WidgetPreference> widgetPreferenceStore;
        private readonly IWidgetStatusChecker widgetStatusChecker;

        public GetDemoWidgetPreCheckHandler(
            ILogger<GetDemoWidgetPreCheckHandler> logger,
            IEntityStore<Area> intentStore,
            IEntityStore<WidgetPreference> widgetPreferenceStore,
            IWidgetStatusChecker widgetStatusChecker)
        {
            this.logger = logger;
            this.intentStore = intentStore;
            this.widgetPreferenceStore = widgetPreferenceStore;
            this.widgetStatusChecker = widgetStatusChecker;
        }

        public async Task<GetDemoWidgetPreCheckResponse> Handle(GetDemoWidgetPreCheckRequest request, CancellationToken cancellationToken)
        {
            var widgetPrefs = await widgetPreferenceStore.Get(widgetPreferenceStore.AccountId, s => s.AccountId);
            var intent = await intentStore.GetActiveIntentsWithConvoAndDynamicAndStaticTables();

            var result = await widgetStatusChecker.ExecuteWidgetStatusCheck(intent, widgetPrefs, true, logger);
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