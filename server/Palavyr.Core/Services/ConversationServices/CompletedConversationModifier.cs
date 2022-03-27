using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.ConversationServices
{
    public interface ICompletedConversationModifier
    {
        Task<IEnumerable<Enquiry>> ModifyCompletedConversation(string conversationId);
    }

    public class CompletedConversationModifier : ICompletedConversationModifier
    {
        private readonly ConvoContext convoContext;
        private readonly IConversationRecordRetriever conversationRecordRetriever;
        private readonly IAccountIdTransport accountIdTransport;

        public CompletedConversationModifier(ConvoContext convoContext, IConversationRecordRetriever conversationRecordRetriever, IAccountIdTransport accountIdTransport )
        {
            this.convoContext = convoContext;
            this.conversationRecordRetriever = conversationRecordRetriever;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<IEnumerable<Enquiry>> ModifyCompletedConversation(string conversationId)
        {
            var convo = await convoContext.ConversationRecords.SingleOrDefaultAsync(row => row.AccountId == accountIdTransport.AccountId && row.ConversationId == conversationId);
            convo.Seen = !convo.Seen;
            await convoContext.SaveChangesAsync();

            return await conversationRecordRetriever.RetrieveConversationRecords();
        }
    }
}