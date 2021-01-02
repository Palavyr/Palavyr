using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.ResponseTypes;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Utils
{
    public static class WidgetStatusUtils
    {
        public static async Task<PreCheckResult> ExecuteWidgetStatusCheck(string accountId, DashContext dashContext,
            bool demo, ILogger logger)
        {
            logger.LogDebug($"Get Widget State - should only be one widget associated with account ID {accountId}");
            var prefs = dashContext.WidgetPreferences.Single(row => row.AccountId == accountId);
            var widgetState = prefs.WidgetState;

            logger.LogDebug("Collecting areas...");
            var areas = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .ToListAsync();

            logger.LogDebug("Collected areas.... running pre-check");
            var result = StatusCheck(areas, widgetState, demo, logger);
            return result;
        }

        private static PreCheckResult StatusCheck(List<Area> areas, bool widgetState, bool demo, ILogger logger)
        {
            var incompleteAreas = new List<Area>();
            logger.LogDebug("Attempting RunConversationsPreCheck...");

            var isReady = true;
            foreach (var area in areas)
            {
                var nodeList = area.ConversationNodes.ToArray();
                var requiredNodes = area
                    .DynamicTableMetas
                    .Select(TreeUtils.TransformRequiredNodeType)
                    .ToArray();

                logger.LogDebug($"Required Nodes Found. Number of required nodes: {requiredNodes.Length}");
                List<bool> checks;
                try
                {
                    checks = new List<bool>()
                    {
                        AllNodesAreSet(nodeList),
                        AllBranchesTerminate(nodeList),
                        AllRequiredNodesSatisfied(nodeList, requiredNodes),
                    };
                }
                catch (Exception ex)
                {
                    logger.LogDebug($"Node checks failed: {ex.Message}");
                    throw;
                }

                isReady = checks.TrueForAll(x => x == true);
                logger.LogDebug($"Checked isReady status: {isReady}");
                if (isReady) continue;
                incompleteAreas.Add(area);
                logger.LogDebug($"Area not currently ready: {area.AreaName}");
            }

            logger.LogDebug("Pre-check Complete. Returning result.");
            if (demo)
            {
                return PreCheckResult.CreateConvoResult(incompleteAreas, isReady);
            }

            if (widgetState)
            {
                return PreCheckResult.CreateConvoResult(incompleteAreas, isReady);
            }
            else
            {
                return PreCheckResult.CreateConvoResult(incompleteAreas, false);
            }
        }

        private static bool AllNodesAreSet(ConversationNode[] nodeList)
        {
            var emptyNodeTypes = nodeList.Select(x => string.IsNullOrEmpty(x.NodeType)).ToArray();
            return emptyNodeTypes.All(x => x == false);
        }

        private static bool AllBranchesTerminate(ConversationNode[] nodeList)
        {
            var rootNode = TreeUtils.GetRootNode(nodeList);
            var numLeaves = TreeUtils.TraverseTheTreeFromTheTop(nodeList, rootNode);
            var numTerminal = TreeUtils.GetNumTerminal(nodeList);
            return (numLeaves == numTerminal);
        }

        private static bool AllRequiredNodesSatisfied(ConversationNode[] nodeList, string[] requiredNodes)
        {
            var missingNodes = TreeUtils.GetMissingNodes(nodeList, requiredNodes);
            return missingNodes.Length == 0;
        }
    }
}