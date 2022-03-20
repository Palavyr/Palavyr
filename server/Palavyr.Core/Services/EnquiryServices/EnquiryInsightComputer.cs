using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Nodes;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.EnquiryServices
{
    public interface IEnquiryInsightComputer
    {
        Task<EnquiryInsightsResource[]> GetEnquiryInsights();
    }

    public class EnquiryInsightComputer : IEnquiryInsightComputer
    {
        private readonly IEntityStore<ConversationRecord> convoRecordStore;
        private readonly IEntityStore<ConversationHistory> convoHistoryStore;
        private readonly IEntityStore<Area> intentStore;
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly INodeBranchLengthCalculator nodeBranchLengthCalculator;

        public EnquiryInsightComputer(
            IEntityStore<ConversationRecord> convoRecordStore,
            IEntityStore<ConversationHistory> convoHistoryStore,
            IEntityStore<Area> intentStore,
            IEntityStore<ConversationNode> convoNodeStore,
            INodeBranchLengthCalculator nodeBranchLengthCalculator
        )
        {
            this.convoRecordStore = convoRecordStore;
            this.convoHistoryStore = convoHistoryStore;
            this.intentStore = intentStore;
            this.convoNodeStore = convoNodeStore;
            this.nodeBranchLengthCalculator = nodeBranchLengthCalculator;
        }

        public async Task<EnquiryInsightsResource[]> GetEnquiryInsights()
        {
            var resources = new List<EnquiryInsightsResource>();
            var allRecords = await convoRecordStore.GetAll();
            var allIntents = await intentStore.GetAll();

            foreach (var intent in allIntents)
            {
                var intentRecords = allRecords.Where(x => x.AreaIdentifier == intent.AreaIdentifier).ToArray();

                var completed = intentRecords.Where(x => x.IsComplete).ToArray().Length;
                var numRecords = intentRecords.Length;
                var sentEmailCount = intentRecords.Where(x => !string.IsNullOrEmpty(x.EmailTemplateUsed)).ToArray().Length;

                var (completePerIntent, averageIntentCompletion) = await ComputeAverageIntentCompletion(intentRecords);
                resources.Add(
                    new EnquiryInsightsResource
                    {
                        IntentName = intent.AreaName,
                        NumRecords = numRecords,
                        IntentIdentifier = intent.AreaIdentifier,
                        Completed = completed,
                        SentEmailCount = sentEmailCount,
                        IntentCompletePerIntent = completePerIntent,
                        AverageIntentCompletion = averageIntentCompletion
                    });
            }

            return resources.ToArray();
        }

        private async Task<(List<double> completePerIntent, double)> ComputeAverageIntentCompletion(ConversationRecord[] intentRecords)
        {
            var completePerIntent = new List<double>();

            foreach (var intentRecord in intentRecords)
            {
                var convo = await convoHistoryStore.GetMany(intentRecord.ConversationId, s => s.ConversationId);
                if (convo.Count == 0) continue;

                var totalConvo = await convoNodeStore.GetMany(intentRecord.AreaIdentifier, s => s.AreaIdentifier);

                var terminalNodes = totalConvo.Where(x => x.IsTerminalType).ToList();
                var lengthOfLongestBranch = nodeBranchLengthCalculator.GetLengthOfLongestTerminatingPath(totalConvo.ToArray(), terminalNodes.ToArray());

                var percentOfConversationCompleted = totalConvo.Count > 0 ? (double) convo.Count / (double) totalConvo.Count : -1;


                completePerIntent.Add(percentOfConversationCompleted);
            }

            var averageIntentCompletion = (double) completePerIntent.Sum() / (double) completePerIntent.Count();

            return (completePerIntent, intentRecords.Length > 3 ? averageIntentCompletion : -1);
        }
    }
}