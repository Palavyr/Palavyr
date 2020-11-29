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
        public static async Task<PreCheckResult> ExecuteWidgetStatusCheck(string accountId, DashContext dashContext, ILogger logger)
        {
            logger.LogDebug("Collecting areas...");
            var areas = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .ToListAsync();

            logger.LogDebug("Collected areas.... running pre-check");
            var result = StatusCheck(areas, logger);
            return result;
        }
        private static PreCheckResult StatusCheck(List<Area> areas, ILogger _logger)
        {
            var incompleteAreas = new List<Area>();
            _logger.LogDebug("Attempting RunConversationsPreCheck...");
            
            var isReady = true;
            foreach (var area in areas)
            {
                var nodeList = area.ConversationNodes.ToArray();
                var requiredNodes = area
                    .DynamicTableMetas
                    .Select(TreeUtils.TransformRequiredNodeType)
                    .ToArray();

                _logger.LogDebug($"Required Nodes Found. Number of required nodes: {requiredNodes.Length}");
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
                    _logger.LogDebug($"Node checks failed: {ex.Message}");
                    throw;
                }

                isReady = checks.TrueForAll(x => x == true);
                _logger.LogDebug($"Checked isReady status: {isReady}");
                if (isReady) continue;
                incompleteAreas.Add(area);
                _logger.LogDebug($"Area not currently ready: {area.AreaName}");
            }
            _logger.LogDebug("Pre-check Complete. Returning result.");
            return PreCheckResult.CreateConvoResult(incompleteAreas, isReady);
        }

        private static bool AllNodesAreSet(ConversationNode[] nodeList)
        {
            var EmptyNodeTypes = nodeList.Select(x => string.IsNullOrEmpty(x.NodeType)).ToArray();
            return EmptyNodeTypes.All(x => x == false);
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