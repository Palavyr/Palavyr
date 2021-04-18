using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.Core.Models
{
    public class WidgetStatusUtils
    {
        private readonly RequiredNodeCalculator requiredNodeCalculator;
        private readonly MissingNodeCalculator missingNodeCalculator;

        public WidgetStatusUtils(
            RequiredNodeCalculator requiredNodeCalculator,
            MissingNodeCalculator missingNodeCalculator
        )
        {
            this.requiredNodeCalculator = requiredNodeCalculator;
            this.missingNodeCalculator = missingNodeCalculator;
        }

        public async Task<PreCheckResult> ExecuteWidgetStatusCheck(
            string accountId,
            List<Area> areas,
            WidgetPreference widgetPreferences,
            bool demo,
            ILogger logger)
        {
            var widgetState = widgetPreferences.WidgetState;
            // dynamic tables might have a 'num individuals' requirement
            // static tables might have a 'num individuals' requirement
            // user may simply wish to collect 'num individuals'

            logger.LogDebug("Collected areas.... running pre-check");
            var result = await StatusCheck(areas, widgetState, demo, logger);
            return result;
        }

        private async Task<PreCheckResult> StatusCheck(List<Area> areas, bool widgetState, bool demo, ILogger logger)
        {
            var incompleteAreas = new List<Area>();
            logger.LogDebug("Attempting RunConversationsPreCheck...");

            var isReady = true;
            foreach (var area in areas)
            {
                var nodeList = area.ConversationNodes.ToArray();
                var allRequiredNodes = (await requiredNodeCalculator.FindRequiredNodes(area)).ToList();

                logger.LogDebug($"Required Nodes Found. Number of required nodes: {allRequiredNodes.Count}");

                var nodesSet = AllNodesAreSet(nodeList);
                var branchesTerminate = AllBranchesTerminate(nodeList);
                var nodesSatisfied = AllRequiredNodesSatisfied(nodeList, allRequiredNodes.ToArray());
                var checks = new List<bool>() {nodesSet, branchesTerminate, nodesSatisfied};

                isReady = checks.TrueForAll(x => x);
                logger.LogDebug($"Checked isReady status: {isReady}");
                if (isReady) continue;

                incompleteAreas.Add(area);
                logger.LogDebug($"Area not currently ready: {area.AreaName}");
            }

            logger.LogDebug("Pre-check Complete. Returning result.");
            if (demo)
            {
                logger.LogDebug("Demo widget activated");
                return PreCheckResult.CreateConvoResult(incompleteAreas, isReady);
            }

            logger.LogDebug("Live Widget activated");
            if (widgetState)
            {
                logger.LogDebug("WidgetState is true");
                return PreCheckResult.CreateConvoResult(incompleteAreas, isReady);
            }
            else
            {
                logger.LogDebug("WidgetState is false");
                return PreCheckResult.CreateConvoResult(incompleteAreas, false);
            }
        }

        private bool AllNodesAreSet(ConversationNode[] nodeList)
        {
            var emptyNodeTypes = nodeList.Select(x => string.IsNullOrEmpty(x.NodeType)).ToArray();
            return emptyNodeTypes.All(x => x == false);
        }

        private bool AllBranchesTerminate(ConversationNode[] nodeList)
        {
            var terminals = nodeList
                .Where(t => t.IsTerminalType)
                .OrderBy(x => x.NodeId)
                .ToList();
            var nodeChilds = nodeList
                .Where(a => string.IsNullOrWhiteSpace(a.NodeChildrenString))
                .OrderBy(x => x.NodeId)
                .ToList();
            return terminals.SequenceEqual(nodeChilds);
        }

        private bool AllRequiredNodesSatisfied(ConversationNode[] nodeList, NodeTypeOption[] requiredNodes)
        {
            var missingNodes = missingNodeCalculator.FindMissingNodes(nodeList, requiredNodes);
            return missingNodes.Length == 0;
        }
    }
}