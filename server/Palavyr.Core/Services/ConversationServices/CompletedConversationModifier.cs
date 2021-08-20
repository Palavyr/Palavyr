using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.Core.Services.ConversationServices
{
    public interface ICompletedConversationModifier
    {
        Task<Enquiry[]> ModifyCompletedConversation(string accountId, string conversationId);
    }

    public class CompletedConversationModifier : ICompletedConversationModifier
    {
        private readonly ConvoContext convoContext;
        private readonly IConversationRecordRetriever conversationRecordRetriever;

        public CompletedConversationModifier(ConvoContext convoContext, IConversationRecordRetriever conversationRecordRetriever)
        {
            this.convoContext = convoContext;
            this.conversationRecordRetriever = conversationRecordRetriever;
        }

        public async Task<Enquiry[]> ModifyCompletedConversation(string accountId, string conversationId)
        {
            var convo = await convoContext.ConversationRecords.SingleOrDefaultAsync(row => row.AccountId == accountId && row.ConversationId == conversationId);
            convo.Seen = !convo.Seen;
            await convoContext.SaveChangesAsync();

            return await conversationRecordRetriever.RetrieveConversationRecords(accountId);
        }
    }
}