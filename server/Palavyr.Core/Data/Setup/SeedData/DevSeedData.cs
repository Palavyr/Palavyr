using System.Collections.Generic;
using Palavyr.Core.Models.Conversation.Schemas;

namespace Palavyr.Core.Data.Setup.SeedData
{
    public class DevSeedData : BaseSeedData
    {
        public List<ConversationRecord> CompleteConversations { get; set; } = new List<ConversationRecord>();

        public DevSeedData(string accountId, string defaultEmail, string introId) : base(accountId, defaultEmail, introId)
        {
            // CompleteConversations.Add(CompletedConversation.CreateNew("conv1-3234-b3jk-kb35", "resp1-2353-3532-345g.pdf", DateTime.Now, accountId, AreaName, EmailTemplate, false, "Toby", "toby@gmail.com", null));
            // CompleteConversations.Add(CompletedConversation.CreateNew("conv2-3234-b3jk-kb35", "resp2-2353-3532-345g.pdf", DateTime.Now, accountId, AreaName, EmailTemplate, false, "Ana", "anagradie@gmail.com", "0449702364"));
        }
    }
}

