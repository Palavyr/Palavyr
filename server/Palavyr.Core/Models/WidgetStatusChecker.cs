using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Nodes;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.PricingStrategyTableServices;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Models
{
    public interface IWidgetStatusChecker
    {
        Task<PreCheckResultResource> ExecuteWidgetStatusCheck(
            List<Intent> intents,
            WidgetPreference widgetPreferences,
            bool demo,
            ILogger logger);
    }

    public class WidgetStatusChecker : IWidgetStatusChecker
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IRequiredNodeCalculator requiredNodeCalculator;
        private readonly IMissingNodeCalculator missingNodeCalculator;
        private readonly INodeOrderChecker nodeOrderChecker;
        private readonly IEntityStore<Account> accountStore;

        private readonly string introSequenceName = "Introduction Sequence";
        private readonly string generalName = "General";

        public WidgetStatusChecker(
            IEntityStore<ConversationNode> convoNodeStore,
            IPricingStrategyTableCompilerOrchestrator orchestrator,
            IRequiredNodeCalculator requiredNodeCalculator,
            IMissingNodeCalculator missingNodeCalculator,
            INodeOrderChecker nodeOrderChecker,
            IEntityStore<Account> accountStore)
        {
            this.convoNodeStore = convoNodeStore;
            this.requiredNodeCalculator = requiredNodeCalculator;
            this.missingNodeCalculator = missingNodeCalculator;
            this.nodeOrderChecker = nodeOrderChecker;
            this.accountStore = accountStore;
        }

        public async Task<PreCheckResultResource> ExecuteWidgetStatusCheck(
            List<Intent> intents,
            WidgetPreference widgetPreferences,
            bool demo,
            ILogger logger)
        {
            var widgetState = widgetPreferences.WidgetState;
            // pricing strategy tables might have a 'num individuals' requirement
            // static tables might have a 'num individuals' requirement
            // user may simply wish to collect 'num individuals'

            logger.LogDebug("Collected intents.... running pre-check");
            var result = await StatusCheck(intents, widgetState, demo, logger);
            return result;
        }

        private async Task<PreCheckResultResource> StatusCheck(List<Intent> intents, bool widgetState, bool demo, ILogger logger)
        {
            logger.LogDebug("Attempting RunConversationsPreCheck...");


            var errors = new List<PreCheckError>();
            var isReady = true;

            var introError = new PreCheckError()
            {
                IntentName = introSequenceName
            };

            var generalError = new PreCheckError
            {
                IntentName = generalName
            };

            var allRequiredIntroNodesArePresent = await AllIntroRequiredIntroNodesArePresent(introError);
            if (!allRequiredIntroNodesArePresent)
            {
                isReady = false;
                introError.Reasons.Add("The introduction sequence has not been completed.");
            }

            var numberOfEnabledIntents = 0;
            foreach (var intent in intents)
            {
                var error = new PreCheckError()
                {
                    IntentName = intent.IntentName
                };

                var nodeList = intent.ConversationNodes.ToArray();
                var allRequiredNodes = (await requiredNodeCalculator.FindRequiredNodes(intent)).ToList();

                logger.LogDebug("Required Nodes Found. Number of required nodes: {NumRequiredNodes}", allRequiredNodes.Count);

                var nodesSet = AllNodesAreSet(nodeList, error);
                var branchesTerminate = AllBranchesTerminate(nodeList, error);
                var nodesSatisfied = AllRequiredNodesSatisfied(nodeList, allRequiredNodes.ToArray(), error);
                var pricingStrategyNodesAreOrdered = PricingStrategyNodesAreOrdered(nodeList, error);
                var allImageNodesHaveImagesSet = AllImageNodesSet(nodeList, error);
                // var allCategoricalPricingStrategiesAreUnique = await AllCategoricalPricingStrategiesAreUnique(intent, error);

                var checks = new List<bool>() { nodesSet, branchesTerminate, nodesSatisfied, pricingStrategyNodesAreOrdered, allImageNodesHaveImagesSet};//, allCategoricalPricingStrategiesAreUnique };

                var intentChecksPassed = checks.TrueForAll(x => x);
                if (!intentChecksPassed)
                {
                    isReady = false;
                    errors.Add(error);
                    logger.LogDebug("Intent not currently ready: {Name}", intent.IntentName);
                }

                if (intent.IsEnabled)
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

            logger.LogDebug("Pre-check Complete. Returning result");
            if (demo)
            {
                logger.LogDebug("Demo widget activated");
                return PreCheckResultResource.CreateConvoResult(isReady, errors);
            }

            logger.LogDebug("Live Widget activated");
            if (widgetState)
            {
                logger.LogDebug("WidgetState is true");
                return PreCheckResultResource.CreateConvoResult(isReady, errors);
            }
            else
            {
                logger.LogDebug("WidgetState is false");
                return PreCheckResultResource.CreateConvoResult(false, errors);
            }
        }


        private async Task<bool> AllIntroRequiredIntroNodesArePresent(PreCheckError error)
        {
            var isReady = true;
            var account = await accountStore.GetAccount();
            var introId = account.IntroIntentId;

            var introSequence = await convoNodeStore.GetMany(introId, s => s.IntentId);
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

        // private async Task<bool> AllCategoricalPricingStrategiesAreUnique(Intent intent, PreCheckError error)
        // {
        //     var pricingStrategies = intent.PricingStrategyTableMetas;
        //     var results = await orchestrator.ValidatePricingStrategies(pricingStrategies);
        //     var ready = true;
        //     if (results.Count > 0)
        //     {
        //         foreach (var result in results)
        //         {
        //             if (!result.IsValid && result.Reasons != null)
        //             {
        //                 ready = false;
        //                 error.Reasons.AddRange(result.Reasons);
        //             }
        //         }
        //     }
        //
        //     return ready;
        // }

        private bool AllImageNodesSet(ConversationNode[] nodeList, PreCheckError error)
        {
            var imageNodes = nodeList.Where(x => x.IsImageNode);
            var count = 0;
            foreach (var imageNode in imageNodes)
            {
                if (imageNode.FileId == null)
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

        private bool PricingStrategyNodesAreOrdered(ConversationNode[] nodeList, PreCheckError error)
        {
            var nodeOrderCheckResult = nodeOrderChecker.AllPricingStrategyTypesAreOrderedCorrectlyByResolveOrder(nodeList);
            if (!nodeOrderCheckResult.IsOrdered)
            {
                error.Reasons.Add("Pricing Strategy Table nodes are not present in the correct order.");
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

        private bool AllRequiredNodesSatisfied(ConversationNode[] nodeList, NodeTypeOptionResource[] requiredNodes, PreCheckError error)
        {
            var missingNodes = missingNodeCalculator.FindMissingNodes(nodeList, requiredNodes);
            var result = missingNodes.Length == 0;
            if (!result)
            {
                error.Reasons.Add($"A total of {missingNodes.Length} Pricing strategy table nodes have not been added.");
            }

            return result;
        }
    }
}