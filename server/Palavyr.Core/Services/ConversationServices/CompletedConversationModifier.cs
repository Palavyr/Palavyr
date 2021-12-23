using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.ConversationServices
{
    public interface ICompletedConversationModifier
    {
        Task<Enquiry[]> ModifyCompletedConversation(string conversationId);
    }

    public class CompletedConversationModifier : ICompletedConversationModifier
    {
        private readonly ConvoContext convoContext;
        private readonly IConversationRecordRetriever conversationRecordRetriever;
        private readonly IHoldAnAccountId accountIdHolder;

        public CompletedConversationModifier(ConvoContext convoContext, IConversationRecordRetriever conversationRecordRetriever, IHoldAnAccountId accountIdHolder )
        {
            this.convoContext = convoContext;
            this.conversationRecordRetriever = conversationRecordRetriever;
            this.accountIdHolder = accountIdHolder;
        }

        public async Task<Enquiry[]> ModifyCompletedConversation(string conversationId)
        {
            var convo = await convoContext.ConversationRecords.SingleOrDefaultAsync(row => row.AccountId == accountIdHolder.AccountId && row.ConversationId == conversationId);
            convo.Seen = !convo.Seen;
            await convoContext.SaveChangesAsync();

            return await conversationRecordRetriever.RetrieveConversationRecords();
        }
    }
}