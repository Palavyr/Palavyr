using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models.Conversation
{
    public interface IConversationNodeUpdater
    {
        Task<List<ConversationNode>> UpdateConversation(string intentId, IEnumerable<ConversationNode> mappedUpdates, CancellationToken cancellationToken);
    }
}