using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Data;
using Palavyr.Domain.Resources.Responses;

namespace Palavyr.Services.ConversationServices
{
    public class CompletedConversationModifier
    {
        private readonly ConvoContext convoContext;
        private readonly CompletedConversationRetriever completedConversationRetriever;

        public CompletedConversationModifier(ConvoContext convoContext, CompletedConversationRetriever completedConversationRetriever)
        {
            this.convoContext = convoContext;
            this.completedConversationRetriever = completedConversationRetriever;
        }

        public async Task<Enquiry[]> ModifyCompletedConversation(string accountId, string conversationId)
        {
            var convo = await convoContext.CompletedConversations.SingleOrDefaultAsync(row => row.AccountId == accountId && row.ConversationId == conversationId);
            convo.Seen = !convo.Seen;
            await convoContext.SaveChangesAsync();

            return await completedConversationRetriever.RetrieveCompletedConversations(accountId);
        }
    }
}