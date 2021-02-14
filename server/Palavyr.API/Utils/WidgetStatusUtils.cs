﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.ResponseTypes;
using Server.Domain.Configuration.Constant;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Utils
{
    public static class WidgetStatusUtils
    {
        public static async Task<PreCheckResult> ExecuteWidgetStatusCheck(
            string accountId,
            DashContext dashContext,
            bool demo,
            ILogger logger
        )
        {
            logger.LogDebug($"Get Widget State - should only be one widget associated with account ID {accountId}");
            var prefs = dashContext.WidgetPreferences.Single(row => row.AccountId == accountId);
            var widgetState = prefs.WidgetState;

            logger.LogDebug("Collecting areas...");
            var areas = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId && row.IsComplete)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .ToListAsync();

            // dynamic tables don't support 'per person' - this must be intrinsic to the question question type.
            // We just check static tables.
            var staticTableRows = await dashContext
                .StaticTablesRows
                .Where(row => row.AccountId == accountId)
                .ToListAsync();

            logger.LogDebug("Collected areas.... running pre-check");
            var result = StatusCheck(areas, staticTableRows, widgetState, demo, logger);
            return result;
        }

        private static PreCheckResult StatusCheck(List<Area> areas, List<StaticTableRow> staticTableRows, bool widgetState, bool demo, ILogger logger)
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
                var perIndividualRequired = staticTableRows
                    .Where(row => row.AreaIdentifier == area.AreaIdentifier)
                    .Select(x => x.PerPerson)
                    .Any();

                var allRequiredNodes = new List<string>(requiredDynamicNodes);
                if (perIndividualRequired)
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

        // private static bool PerIndividualSatisfied(ConversationNode[] nodeList, bool perPersonRequired)
        // {
        //     var branchesWithMissingPerIndividual = TreeUtils.GetMissingNodes(nodeList, new[] {DefaultNodeTypeOptions.TakeNumberIndividuals.StringName});
        //     return perPersonRequired ? branchesWithMissingPerIndividual.Length == 0 : true;
        // }

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