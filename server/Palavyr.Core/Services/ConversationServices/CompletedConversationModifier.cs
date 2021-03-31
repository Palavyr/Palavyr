using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.Core.Services.ConversationServices
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