﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using DashboardServer.Data.Abstractions;
using Microsoft.Extensions.Logging;
using Palavyr.API.CommonResponseTypes;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Utils
{
    public static class WidgetStatusUtils
    {
        public static async Task<PreCheckResult> ExecuteWidgetStatusCheck(
            string accountId,
            IDashConnector dashConnector,
            bool demo,
            ILogger logger
        )
        {
            logger.LogDebug($"Get Widget State - should only be one widget associated with account ID {accountId}");
            var prefs = await dashConnector.GetWidgetPreferences(accountId);
            var widgetState = prefs.WidgetState;

            logger.LogDebug("Collecting areas...");
            var areas = await dashConnector.GetActiveAreasWithConvoAndDynamicAndStaticTables(accountId);

            // dynamic tables might have a 'num individuals' requirement
            // static tables might have a 'num individuals' requirement
            // user may simply wish to collect 'num individuals'

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

                // dynamic node types are required
                var requiredDynamicNodes = area
                    .DynamicTableMetas
                    .Select(TreeUtils.TransformRequiredNodeType)
                    .ToList();

                // check static tables and dynamic tables to see if even 1 'per individual' is set. If so, then check for this node type.
                var perIndividualRequiredStaticTables = area
                    .StaticTablesMetas
                    .SelectMany(x => x.StaticTableRows)
                    .Select(x => x.PerPersonInputRequired)
                    .Any(p => p);

                var allRequiredNodes = new List<string>(requiredDynamicNodes);
                if (perIndividualRequiredStaticTables && !allRequiredNodes.Contains(DefaultNodeTypeOptions.TakeNumberIndividuals.StringName))
                {
                    allRequiredNodes.Add(DefaultNodeTypeOptions.TakeNumberIndividuals.StringName);
                }

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

                // var dynamicAreasMissing = TreeUtils.GetMissingNodes(nodeList, requiredDynamicNodes.ToArray());
                // var perIndividualMissing = TreeUtils.GetMissingNodes(nodeList, new[] {DefaultNodeTypeOptions.TakeNumberIndividuals.StringName});

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