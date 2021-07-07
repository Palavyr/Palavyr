using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models.Conversation
{
    public interface IConversationNodeUpdater
    {
        Task<List<ConversationNode>> UpdateConversation(string accountId, string areaId, List<ConversationNode> updatedConvo, CancellationToken cancellationToken);
    }
}