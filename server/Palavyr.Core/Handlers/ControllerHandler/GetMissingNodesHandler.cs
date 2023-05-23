using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
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
        private readonly IMapToNew<ConversationNodeResource, ConversationNode> conversationNodeMapper;

        public GetMissingNodesHandler(
            ILogger<GetMissingNodesHandler> logger,
            IEntityStore<Intent> intentStore,
            IRequiredNodeCalculator requiredNodeCalculator,
            IMissingNodeCalculator missingNodeCalculator,
            INodeOrderChecker nodeOrderChecker,
            IMapToNew<ConversationNodeResource, ConversationNode> conversationNodeMapper)
        {
            this.logger = logger;
            this.intentStore = intentStore;
            this.requiredNodeCalculator = requiredNodeCalculator;
            this.missingNodeCalculator = missingNodeCalculator;
            this.nodeOrderChecker = nodeOrderChecker;
            this.conversationNodeMapper = conversationNodeMapper;
        }

        public async Task<GetMissingNodesResponse> Handle(GetMissingNodesRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.GetIntentComplete(request.IntentId);

            var pricingStrategyTableMetas = intent.PricingStrategyTableMetas;
            var staticTableMetas = intent.StaticTablesMetas;
            var requiredPricingStrategyNodeTypes = await requiredNodeCalculator.FindRequiredNodes(intent);

            var transactions = (await conversationNodeMapper.MapMany(request.Transactions, cancellationToken)).ToArray();
            var allMissingNodeTypeNames = missingNodeCalculator.CalculateMissingNodes(requiredPricingStrategyNodeTypes.ToArray(), transactions.ToList(), pricingStrategyTableMetas, staticTableMetas);
            var nodeOrderCheckResult = nodeOrderChecker.AllPricingStrategyTypesAreOrderedCorrectlyByResolveOrder(transactions);
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
        public List<ConversationNodeResource> Transactions { get; set; }
        public string IntentId { get; set; }
    }
}