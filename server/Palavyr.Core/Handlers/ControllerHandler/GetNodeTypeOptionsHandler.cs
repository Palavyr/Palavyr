using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Services.PricingStrategyTableServices;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetNodeTypeOptionsHandler : IRequestHandler<GetNodeTypeOptionsRequest, GetNodeTypeOptionsResponse>
    {
        private readonly ILogger<GetNodeTypeOptionsHandler> logger;
        private readonly IPricingStrategyTableCompilerOrchestrator pricingStrategyTableCompilerOrchestrator;
        private readonly IEntityStore<PricingStrategyTableMeta> dynamicTableMetaStore;

        public GetNodeTypeOptionsHandler(
            ILogger<GetNodeTypeOptionsHandler> logger,
            IPricingStrategyTableCompilerOrchestrator pricingStrategyTableCompilerOrchestrator,
            IEntityStore<PricingStrategyTableMeta> dynamicTableMetaStore)
        {
            this.logger = logger;
            this.pricingStrategyTableCompilerOrchestrator = pricingStrategyTableCompilerOrchestrator;
            this.dynamicTableMetaStore = dynamicTableMetaStore;
        }

        public async Task<GetNodeTypeOptionsResponse> Handle(GetNodeTypeOptionsRequest request, CancellationToken cancellationToken)
        {
            var dynamicTableMetas = await dynamicTableMetaStore.GetMany(request.IntentId, s => s.AreaIdentifier);
            var dynamicTableData = await pricingStrategyTableCompilerOrchestrator.CompileTablesToConfigurationNodes(dynamicTableMetas, request.IntentId);
            var defaultNodeTypeOptions = DefaultNodeTypeOptions.DefaultNodeTypeOptionsList;

            var fullNodeTypeOptionsList = defaultNodeTypeOptions.AddAdditionalNodes(dynamicTableData);

            return new GetNodeTypeOptionsResponse(fullNodeTypeOptionsList);
        }
    }

    public class GetNodeTypeOptionsResponse
    {
        public GetNodeTypeOptionsResponse(IEnumerable<NodeTypeOptionResource> response) => Response = response;
        public IEnumerable<NodeTypeOptionResource> Response { get; set; }
    }

    public class GetNodeTypeOptionsRequest : IRequest<GetNodeTypeOptionsResponse>
    {
        public GetNodeTypeOptionsRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}