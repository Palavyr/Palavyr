using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Nodes;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetMissingNodesHandler : IRequestHandler<GetMissingNodesRequest, GetMissingNodesResponse>
    {
        private readonly ILogger<GetMissingNodesHandler> logger;
        private readonly IEntityStore<Intent> intentStore;
        private readonly IRequiredNodeCalculator requiredNodeCalculator;
        private readonly IMissingNodeCalculator missingNodeCalculator;
        private readonly INodeOrderChecker nodeOrderChecker;

        public GetMissingNodesHandler(
            ILogger<GetMissingNodesHandler> logger,
            IEntityStore<Intent> intentStore,
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
            var intent = await intentStore.GetIntentComplete(request.IntentId);

            var dynamicTableMetas = intent.DynamicTableMetas;
            var staticTableMetas = intent.StaticTablesMetas;

            var requiredDynamicNodeTypes = await requiredNodeCalculator.FindRequiredNodes(intent);
            var allMissingNodeTypeNames = missingNodeCalculator.CalculateMissingNodes(requiredDynamicNodeTypes.ToArray(), request.Transactions, dynamicTableMetas, staticTableMetas);
            var nodeOrderCheckResult = nodeOrderChecker.AllDynamicTypesAreOrderedCorrectlyByResolveOrder(request.Transactions.ToArray());
            var errorResponse = new TreeErrorsResource(allMissingNodeTypeNames, nodeOrderCheckResult.ConcatenatedNodeTypes.ToArray());
            return new GetMissingNodesResponse(errorResponse);
        }
    }

    public class GetMissingNodesResponse
    {
        public GetMissingNodesResponse(TreeErrorsResource resource) => Resource = resource;
        public TreeErrorsResource Resource { get; set; }
    }

    public class GetMissingNodesRequest : IRequest<GetMissingNodesResponse>
    {
        public List<ConversationNode> Transactions { get; set; }
        public string IntentId { get; set; }
    }
}