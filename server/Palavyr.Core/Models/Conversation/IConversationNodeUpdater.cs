using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Models.Conversation
{
    public interface IConversationNodeUpdater
    {
        Task<List<ConversationNode>> UpdateDesignerConversationForIntent(string intentId, IEnumerable<ConversationNode> mappedUpdates, CancellationToken cancellationToken);
    }
}