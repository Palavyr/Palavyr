using System;
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
        private readonly TreeRootFinder treeRootFinder;
        private readonly TreeWalker treeWalker;
        private readonly NodeCounter nodeCounter;

        public WidgetStatusUtils(
            RequiredNodeCalculator requiredNodeCalculator,
            MissingNodeCalculator missingNodeCalculator,
            TreeRootFinder treeRootFinder,
            TreeWalker treeWalker,
            NodeCounter nodeCounter
        )
        {
            this.requiredNodeCalculator = requiredNodeCalculator;
            this.missingNodeCalculator = missingNodeCalculator;
            this.treeRootFinder = treeRootFinder;
            this.treeWalker = treeWalker;
            this.nodeCounter = nodeCounter;
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
                List<bool> checks;
                try
                {
                    checks = new List<bool>()
                    {
                        AllNodesAreSet(nodeList),
                        AllBranchesTerminate(nodeList),
                        AllRequiredNodesSatisfied(nodeList, allRequiredNodes.ToArray()),
                    };
                }
                catch (Exception ex)
                {
                    logger.LogDebug($"Node checks failed: {ex.Message}");
                    throw;
                }

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
            var terminalNodes = new List<ConversationNode>();

            var rootNode = treeRootFinder.GetRootNode(nodeList);
            treeWalker.FindAllTerminalNodes(nodeList, rootNode, terminalNodes); // This is not working correctly. Shouldn't need to distinct on this result... (except maybe for anabranch and split merge

            var uniqueTerminalNodes = terminalNodes.Distinct().ToList();
            var numLeaves = uniqueTerminalNodes.Count();
            var numTerminal = nodeCounter.CountNumTerminal(nodeList);
            return (numLeaves == numTerminal);
        }

        private bool AllRequiredNodesSatisfied(ConversationNode[] nodeList, NodeTypeOption[] requiredNodes)
        {
            var missingNodes = missingNodeCalculator.FindMissingNodes(nodeList, requiredNodes);
            return missingNodes.Length == 0;
        }
    }
}