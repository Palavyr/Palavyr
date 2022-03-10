using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Nodes;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.DynamicTableService;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Models
{
    public interface IWidgetStatusChecker
    {
        Task<PreCheckResult> ExecuteWidgetStatusCheck(
            List<Area> areas,
            WidgetPreference widgetPreferences,
            bool demo,
            ILogger logger);
    }

    public class WidgetStatusChecker : IWidgetStatusChecker
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IDynamicTableCompilerOrchestrator orchestrator;
        private readonly RequiredNodeCalculator requiredNodeCalculator;
        private readonly MissingNodeCalculator missingNodeCalculator;
        private readonly INodeOrderChecker nodeOrderChecker;
        private readonly IEntityStore<Account> accountStore;

        private readonly string introSequenceName = "Introduction Sequence";
        private readonly string generalName = "General";

        public WidgetStatusChecker(
            IEntityStore<ConversationNode> convoNodeStore,
            IDynamicTableCompilerOrchestrator orchestrator,
            RequiredNodeCalculator requiredNodeCalculator,
            MissingNodeCalculator missingNodeCalculator,
            INodeOrderChecker nodeOrderChecker,
            IEntityStore<Account> accountStore)
        {
            this.convoNodeStore = convoNodeStore;
            this.orchestrator = orchestrator;
            this.requiredNodeCalculator = requiredNodeCalculator;
            this.missingNodeCalculator = missingNodeCalculator;
            this.nodeOrderChecker = nodeOrderChecker;
            this.accountStore = accountStore;
        }

        public async Task<PreCheckResult> ExecuteWidgetStatusCheck(
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
            logger.LogDebug("Attempting RunConversationsPreCheck...");


            var errors = new List<PreCheckError>();
            var isReady = true;

            var introError = new PreCheckError()
            {
                AreaName = introSequenceName
            };

            var generalError = new PreCheckError
            {
                AreaName = generalName
            };

            var allRequiredIntroNodesArePresent = await AllIntroRequiredIntroNodesArePresent(introError);
            if (!allRequiredIntroNodesArePresent)
            {
                isReady = false;
                introError.Reasons.Add("The introduction sequence has not been completed.");
                // errors.Add(introError);
            }

            var numberOfEnabledIntents = 0;
            foreach (var area in areas)
            {
                var error = new PreCheckError()
                {
                    AreaName = area.AreaName
                };

                var nodeList = area.ConversationNodes.ToArray();
                var allRequiredNodes = (await requiredNodeCalculator.FindRequiredNodes(area)).ToList();

                logger.LogDebug($"Required Nodes Found. Number of required nodes: {allRequiredNodes.Count}");

                var nodesSet = AllNodesAreSet(nodeList, error);
                var branchesTerminate = AllBranchesTerminate(nodeList, error);
                var nodesSatisfied = AllRequiredNodesSatisfied(nodeList, allRequiredNodes.ToArray(), error);
                var dynamicNodesAreOrdered = DynamicNodesAreOrdered(nodeList, error);
                var allImageNodesHaveImagesSet = AllImageNodesSet(nodeList, error);
                var allCategoricalPricingStrategiesAreUnique = await AllCategoricalPricingStrategiesAreUnique(area, error);

                var checks = new List<bool>() { nodesSet, branchesTerminate, nodesSatisfied, dynamicNodesAreOrdered, allImageNodesHaveImagesSet, allCategoricalPricingStrategiesAreUnique };

                var areaChecksPassed = checks.TrueForAll(x => x);
                if (!areaChecksPassed)
                {
                    isReady = false;
                    errors.Add(error);
                    logger.LogDebug($"Area not currently ready: {area.AreaName}");
                }

                if (area.IsEnabled)
                {
                    numberOfEnabledIntents++;
                }
            }

            if (!(numberOfEnabledIntents >= 1))
            {
                isReady = false;
                generalError.Reasons.Add("At least 1 intent must be enabled.");
            }

            if (introError.Reasons.Count > 0)
            {
                errors.Insert(0, introError);
            }

            if (generalError.Reasons.Count > 0)
            {
                if (introError.Reasons.Count > 0)
                {
                    errors.Insert(1, generalError);
                }
                else
                {
                    errors.Insert(0, generalError);
                }
            }

            logger.LogDebug("Pre-check Complete. Returning result.");
            if (demo)
            {
                logger.LogDebug("Demo widget activated");
                return PreCheckResult.CreateConvoResult(isReady, errors);
            }

            logger.LogDebug("Live Widget activated");
            if (widgetState)
            {
                logger.LogDebug("WidgetState is true");
                return PreCheckResult.CreateConvoResult(isReady, errors);
            }
            else
            {
                logger.LogDebug("WidgetState is false");
                return PreCheckResult.CreateConvoResult(false, errors);
            }
        }


        private async Task<bool> AllIntroRequiredIntroNodesArePresent(PreCheckError error)
        {
            var isReady = true;
            var account = await accountStore.GetAccount();
            var introId = account.IntroductionId;

            var introSequence = await convoNodeStore.GetMany(introId, s => s.AreaIdentifier);
            if (!introSequence.Select(x => x.NodeType).Contains(DefaultNodeTypeOptions.Selection.StringName))
            {
                isReady = false;
                error.Reasons.Add($"Missing {DefaultNodeTypeOptions.CreateSelection().Text}");
            }

            if (!introSequence.Select(x => x.NodeType).Contains(DefaultNodeTypeOptions.CollectDetails.StringName))
            {
                isReady = false;
                error.Reasons.Add($"Missing {DefaultNodeTypeOptions.CreateCollectDetails().Text}");
            }

            return isReady;
        }

        private async Task<bool> AllCategoricalPricingStrategiesAreUnique(Area area, PreCheckError error)
        {
            var pricingStrategies = area.DynamicTableMetas;
            var results = await orchestrator.ValidatePricingStrategies(pricingStrategies);
            var ready = true;
            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    if (!result.IsValid && result.Reasons != null)
                    {
                        ready = false;
                        error.Reasons.AddRange(result.Reasons);
                    }
                }
            }

            return ready;
        }

        private bool AllImageNodesSet(ConversationNode[] nodeList, PreCheckError error)
        {
            var imageNodes = nodeList.Where(x => x.IsImageNode);
            var count = 0;
            foreach (var imageNode in imageNodes)
            {
                if (imageNode.ImageId == null)
                {
                    count++;
                }
            }

            if (count > 0)
            {
                error.Reasons.Add($"A total of {count} image nodes do not have images set.");
                return false;
            }

            return true;
        }

        private bool DynamicNodesAreOrdered(ConversationNode[] nodeList, PreCheckError error)
        {
            var nodeOrderCheckResult = nodeOrderChecker.AllDynamicTypesAreOrderedCorrectlyByResolveOrder(nodeList);
            if (!nodeOrderCheckResult.IsOrdered)
            {
                error.Reasons.Add("Dynamic Table nodes are not present in the correct order.");
            }

            return nodeOrderCheckResult.IsOrdered;
        }

        private bool AllNodesAreSet(ConversationNode[] nodeList, PreCheckError error)
        {
            var emptyNodeTypes = nodeList.Select(x => string.IsNullOrEmpty(x.NodeType)).ToArray();
            var result = emptyNodeTypes.All(x => x == false);
            if (!result)
            {
                error.Reasons.Add("All nodes are not set.");
            }

            return result;
        }

        private bool AllBranchesTerminate(ConversationNode[] nodeList, PreCheckError error)
        {
            // TODO: This is really fucking weird that I wrote this. Why would all Terminal Nodes ever want, need or simple be equal to nodeChilds

            // var allTerminalNodes = nodeList
            //     .Where(t => t.IsTerminalType)
            //     .OrderBy(x => x.NodeId)
            //     .ToList();
            // var nodeChilds = nodeList
            //     .Where(a => string.IsNullOrWhiteSpace(a.NodeChildrenString) && a.NodeType != "Loopback")
            //     .OrderBy(x => x.NodeId)
            //     .ToList();
            // var sequencesAreEqual = allTerminalNodes.SequenceEqual(nodeChilds);
            // if (!sequencesAreEqual)
            // {
            //     error.Reasons.Add("All branches do not terminate.");
            // }
            var terminalTypes = DefaultNodeTypeOptions
                .DefaultNodeTypeOptionsList
                .Where(x => x.IsTerminalType || x.Value == nameof(DefaultNodeTypeOptions.Loopback))
                .Select(x => x.Value).ToList();

            var hangingNodes = nodeList
                .Where(a => string.IsNullOrWhiteSpace(a.NodeChildrenString) && !terminalTypes.Contains(a.NodeType)) // a.NodeType != "Loopback")
                .OrderBy(x => x.NodeId)
                .ToList();
            var noHangingNodes = hangingNodes.Count == 0;
            if (noHangingNodes)
            {
                return noHangingNodes;
            }

            return false;
        }

        private bool AllRequiredNodesSatisfied(ConversationNode[] nodeList, NodeTypeOption[] requiredNodes, PreCheckError error)
        {
            var missingNodes = missingNodeCalculator.FindMissingNodes(nodeList, requiredNodes);
            var result = missingNodes.Length == 0;
            if (!result)
            {
                error.Reasons.Add($"A total of {missingNodes.Length} Dynamic Table nodes have not been added.");
            }

            return result;
        }
    }
}