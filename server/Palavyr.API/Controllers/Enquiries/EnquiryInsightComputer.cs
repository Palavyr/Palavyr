using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Nodes;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.ConversationServices;

namespace Palavyr.API.Controllers.Enquiries
{
    public interface IEnquiryInsightComputer
    {
        Task<EnquiryInsightsResource[]> GetEnquiryInsights(string accountId);
    }

    public class EnquiryInsightComputer : IEnquiryInsightComputer
    {
        private readonly IConvoHistoryRepository convoHistoryRepository;
        private readonly IConversationRecordRetriever conversationRecordRetriever;
        private readonly IConfigurationRepository configurationRepository;
        private readonly INodeBranchLengthCalculator nodeBranchLengthCalculator;

        public EnquiryInsightComputer(
            IConvoHistoryRepository convoHistoryRepository,
            IConversationRecordRetriever conversationRecordRetriever,
            IConfigurationRepository configurationRepository,
            INodeBranchLengthCalculator nodeBranchLengthCalculator
        )
        {
            this.convoHistoryRepository = convoHistoryRepository;
            this.conversationRecordRetriever = conversationRecordRetriever;
            this.configurationRepository = configurationRepository;
            this.nodeBranchLengthCalculator = nodeBranchLengthCalculator;
        }

        public async Task<EnquiryInsightsResource[]> GetEnquiryInsights(string accountId)
        {
            var resources = new List<EnquiryInsightsResource>();
            var allRecords = await convoHistoryRepository.GetAllConversationRecords(accountId);
            var allIntents = await configurationRepository.GetAllAreasShallow(accountId);
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
                var convo = await convoHistoryRepository.GetConversationById(intentRecord.ConversationId);
                if (convo.Length == 0) continue;

                var totalConvo = await configurationRepository.GetAreaConversationNodes(intentRecord.AccountId, intentRecord.AreaIdentifier);

                var terminalNodes = totalConvo.Where(x => x.IsTerminalType).ToList();
                var lengthOfLongestBranch = nodeBranchLengthCalculator.GetLengthOfLongestTerminatingPath(totalConvo.ToArray(), terminalNodes.ToArray());

                var percentOfConversationCompleted = totalConvo.Count > 0 ? (double) convo.Length / (double) totalConvo.Count : -1;


                completePerIntent.Add(percentOfConversationCompleted);
            }

            var averageIntentCompletion = (double) completePerIntent.Sum() / (double) completePerIntent.Count();

            return (completePerIntent, intentRecords.Length > 3 ? averageIntentCompletion : -1);
        }
    }
}