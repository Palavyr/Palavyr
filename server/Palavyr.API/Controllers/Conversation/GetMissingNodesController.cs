using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Nodes;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Conversation
{
    public class GetMissingNodesController : PalavyrBaseController
    {
        private ILogger<GetMissingNodesController> logger;
        private readonly IConfigurationRepository configurationRepository;
        private readonly RequiredNodeCalculator requiredNodeCalculator;
        private readonly MissingNodeCalculator missingNodeCalculator;
        private readonly NodeOrderChecker nodeOrderChecker;

        public GetMissingNodesController(
            ILogger<GetMissingNodesController> logger,
            IConfigurationRepository configurationRepository,
            RequiredNodeCalculator requiredNodeCalculator,
            MissingNodeCalculator missingNodeCalculator,
            NodeOrderChecker nodeOrderChecker
        )
        {
            this.configurationRepository = configurationRepository;
            this.requiredNodeCalculator = requiredNodeCalculator;
            this.missingNodeCalculator = missingNodeCalculator;
            this.nodeOrderChecker = nodeOrderChecker;
            this.logger = logger;
        }

        [HttpPost("configure-conversations/{areaId}/tree-errors")]
        public async Task<TreeErrorsResponse> Get(
            [FromRoute]
            string areaId,
            [FromHeader]
            string accountId,
            [FromBody]
            ConversationNodeDto currentNodes)
        {
            var area = await configurationRepository.GetAreaComplete(accountId, areaId);

            var dynamicTableMetas = area.DynamicTableMetas;
            var staticTableMetas = area.StaticTablesMetas;

            var requiredDynamicNodeTypes = await requiredNodeCalculator.FindRequiredNodes(area);
            var allMissingNodeTypeNames = missingNodeCalculator.CalculateMissingNodes(requiredDynamicNodeTypes.ToArray(), currentNodes.Transactions, dynamicTableMetas, staticTableMetas);
            var nodeOrderCheckResult = nodeOrderChecker.AllDynamicTypesAreOrderedCorrectlyByResolveOrder(currentNodes.Transactions.ToArray());
            return new TreeErrorsResponse(allMissingNodeTypeNames, nodeOrderCheckResult.ConcatenatedNodeTypes.ToArray());
        }

        [HttpPost("configure-conversations/intro/{introId}/tree-errors")]
        public async Task<TreeErrorsResponse> GetIntro(
            [FromRoute]
            string introId,
            [FromHeader]
            string accountId,
            [FromBody]
            ConversationNodeDto currentNodes)
        {
            await Task.CompletedTask;
            var missingNodes = new List<string> { };
            if (!currentNodes.Transactions.Select(x => x.NodeType).Contains(DefaultNodeTypeOptions.Selection.StringName))
            {
                missingNodes.Add(DefaultNodeTypeOptions.Selection.StringName);
            }

            if (!currentNodes.Transactions.Select(x => x.NodeType).Contains(DefaultNodeTypeOptions.CollectDetails.StringName))
            {
                missingNodes.Add(DefaultNodeTypeOptions.CollectDetails.StringName);
            }

            return new TreeErrorsResponse(missingNodes.ToArray(), new string[] { });
        }
    }
}