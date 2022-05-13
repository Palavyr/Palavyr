using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Nodes;
using Palavyr.Core.Resources.Responses;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetMissingNodesHandler : IRequestHandler<GetMissingNodesRequest, GetMissingNodesResponse>
    {
        private readonly ILogger<GetMissingNodesHandler> logger;
        private readonly IEntityStore<Area> intentStore;
        private readonly IRequiredNodeCalculator requiredNodeCalculator;
        private readonly IMissingNodeCalculator missingNodeCalculator;
        private readonly INodeOrderChecker nodeOrderChecker;

        public GetMissingNodesHandler(
            ILogger<GetMissingNodesHandler> logger,
            IEntityStore<Area> intentStore,
            IRequiredNodeCalculator requiredNodeCalculator,
            IMissingNodeCalculator missingNodeCalculator,
            INodeOrderChecker nodeOrderChecker
        )
        {
            this.logger = logger;
            this.intentStore = intentStore;
            this.requiredNodeCalculator = requiredNodeCalculator;
            this.missingNodeCalculator = missingNodeCalculator;
            this.nodeOrderChecker = nodeOrderChecker;
        }

        public async Task<GetMissingNodesResponse> Handle(GetMissingNodesRequest request, CancellationToken cancellationToken)
        {
            var area = await intentStore.GetIntentComplete(request.IntentId);

            var dynamicTableMetas = area.DynamicTableMetas;
            var staticTableMetas = area.StaticTablesMetas;

            var requiredDynamicNodeTypes = await requiredNodeCalculator.FindRequiredNodes(area);
            var allMissingNodeTypeNames = missingNodeCalculator.CalculateMissingNodes(requiredDynamicNodeTypes.ToArray(), request.Transactions, dynamicTableMetas, staticTableMetas);
            var nodeOrderCheckResult = nodeOrderChecker.AllDynamicTypesAreOrderedCorrectlyByResolveOrder(request.Transactions.ToArray());
            var errorResponse = new TreeErrorsResponse(allMissingNodeTypeNames, nodeOrderCheckResult.ConcatenatedNodeTypes.ToArray());
            return new GetMissingNodesResponse(errorResponse);
        }
    }

    public class GetMissingNodesResponse
    {
        public GetMissingNodesResponse(TreeErrorsResponse response) => Response = response;
        public TreeErrorsResponse Response { get; set; }
    }

    public class GetMissingNodesRequest : IRequest<GetMissingNodesResponse>
    {
        public List<ConversationNode> Transactions { get; set; }
        public string IntentId { get; set; }
    }
}