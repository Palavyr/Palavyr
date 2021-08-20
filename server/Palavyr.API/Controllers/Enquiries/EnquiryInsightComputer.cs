using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Conversation.Schemas;
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

        public EnquiryInsightComputer(
            IConvoHistoryRepository convoHistoryRepository,
            IConversationRecordRetriever conversationRecordRetriever,
            IConfigurationRepository configurationRepository
        )
        {
            this.convoHistoryRepository = convoHistoryRepository;
            this.conversationRecordRetriever = conversationRecordRetriever;
            this.configurationRepository = configurationRepository;
        }

        public async Task<EnquiryInsightsResource[]> GetEnquiryInsights(string accountId)
        {
            var resources = new List<EnquiryInsightsResource>();
            var allRecords = await convoHistoryRepository.GetAllConversationRecords(accountId);
            var allIntents = await configurationRepository.GetAllAreasShallow(accountId);
            foreach (var intent in allIntents)
            {
                var intentRecords = allRecords.Where(x => x.AreaIdentifier == intent.AreaIdentifier).ToArray();

                var completed = intentRecords.Select(x => x.IsComplete).ToArray().Length;
                var numRecords = intentRecords.Length;
                var sentEmailCount = intentRecords.Select(x => !string.IsNullOrEmpty(x.EmailTemplateUsed)).ToArray().Length;

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

        private async Task<(List<long> completePerIntent, long)> ComputeAverageIntentCompletion(ConversationRecord[] intentRecords)
        {
            var completePerIntent = new List<long>();

            foreach (var intentRecord in intentRecords)
            {
                var convo = await convoHistoryRepository.GetConversationById(intentRecord.ConversationId);
                var totalConvo = await configurationRepository.GetAreaConversationNodes(intentRecord.AccountId, intentRecord.AreaIdentifier);
                var percentOfConversationCompleted = totalConvo.Count > 0 ? convo.Length / totalConvo.Count : -1;
                completePerIntent.Add(percentOfConversationCompleted);
            }

            return (completePerIntent, completePerIntent.Count() > 0 ? completePerIntent.Sum() / completePerIntent.Count() : -1);
        }
    }
}