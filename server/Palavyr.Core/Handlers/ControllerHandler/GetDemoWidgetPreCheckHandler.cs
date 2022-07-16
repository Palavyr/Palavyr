using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetDemoWidgetPreCheckHandler : IRequestHandler<GetDemoWidgetPreCheckRequest, GetDemoWidgetPreCheckResponse>
    {
        private readonly ILogger<GetDemoWidgetPreCheckHandler> logger;
        private readonly IEntityStore<Intent> intentStore;
        private readonly IEntityStore<WidgetPreference> widgetPreferenceStore;
        private readonly IWidgetStatusChecker widgetStatusChecker;

        public GetDemoWidgetPreCheckHandler(
            ILogger<GetDemoWidgetPreCheckHandler> logger,
            IEntityStore<Intent> intentStore,
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