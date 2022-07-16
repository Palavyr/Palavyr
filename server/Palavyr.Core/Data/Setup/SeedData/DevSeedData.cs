using System.Collections.Generic;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Data.Setup.SeedData
{
    public class DevSeedData : BaseSeedData
    {
        public List<ConversationHistoryMeta> CompleteConversations { get; set; } = new List<ConversationHistoryMeta>();

        public DevSeedData(string accountId, string defaultEmail, string introId) : base(accountId, defaultEmail, introId)
        {
        }
    }
}