using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class ConversationHistoryRowResourceMapper : IMapToNew<ConversationHistoryRow, ConversationHistoryRowResource>
    {
        public async Task<ConversationHistoryRowResource> Map(ConversationHistoryRow from, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            var newResource = new ConversationHistoryRowResource
            {
                ConversationId = from.ConversationId,
                Prompt = from.Prompt,
                UserResponse = from.UserResponse,
                NodeId = from.NodeId,
                NodeCritical = from.NodeCritical,
                NodeType = from.NodeType,
                TimeStamp = from.TimeStamp.ToString(),
                AccountId = from.AccountId
            };

            return newResource;
        }
    }
}