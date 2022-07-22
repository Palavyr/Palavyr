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
        private readonly IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore;

        public GetNodeTypeOptionsHandler(
            ILogger<GetNodeTypeOptionsHandler> logger,
            IPricingStrategyTableCompilerOrchestrator pricingStrategyTableCompilerOrchestrator,
            IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore)
        {
            this.logger = logger;
            this.pricingStrategyTableCompilerOrchestrator = pricingStrategyTableCompilerOrchestrator;
            this.pricingStrategyTableMetaStore = pricingStrategyTableMetaStore;
        }

        public async Task<GetNodeTypeOptionsResponse> Handle(GetNodeTypeOptionsRequest request, CancellationToken cancellationToken)
        {
            var pricingStrategyTableMetas = await pricingStrategyTableMetaStore.GetMany(request.IntentId, s => s.IntentId);
            var pricingStrategyTableData = await pricingStrategyTableCompilerOrchestrator.CompileTablesToConfigurationNodes(pricingStrategyTableMetas, request.IntentId);
            var defaultNodeTypeOptions = DefaultNodeTypeOptions.DefaultNodeTypeOptionsList;

            var fullNodeTypeOptionsList = defaultNodeTypeOptions.AddAdditionalNodes(pricingStrategyTableData);

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