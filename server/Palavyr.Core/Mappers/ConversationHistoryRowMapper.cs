using System;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Mappers
{
    public class ConversationHistoryRowMapper : IMapToNew<ConversationHistoryRowResource, ConversationHistoryRow>
    {
        private readonly IAccountIdTransport accountIdTransport;

        public ConversationHistoryRowMapper(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<ConversationHistoryRow> Map(ConversationHistoryRowResource from, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return new ConversationHistoryRow
            {
                // Creating new so no Id
                AccountId = accountIdTransport.AccountId,
                ConversationId = from.ConversationId,
                NodeCritical = from.NodeCritical,
                NodeId = from.NodeId,
                NodeType = from.NodeType,
                Prompt = from.Prompt,
                TimeStamp = DateTime.Now,
                UserResponse = from.UserResponse
            };
        }
    }
}