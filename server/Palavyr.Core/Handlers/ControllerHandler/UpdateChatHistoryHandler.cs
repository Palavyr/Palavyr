using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class UpdateChatHistoryHandler : INotificationHandler<UpdateChatHistoryRequest>
    {
        private readonly IEntityStore<ConversationHistoryRow> convoHistoryStore;
        private readonly ILogger<UpdateChatHistoryHandler> logger;
        private readonly IAccountIdTransport accountIdTransport;

        public UpdateChatHistoryHandler(IEntityStore<ConversationHistoryRow> convoHistoryStore, ILogger<UpdateChatHistoryHandler> logger, IAccountIdTransport accountIdTransport)
        {
            this.convoHistoryStore = convoHistoryStore;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task Handle(UpdateChatHistoryRequest request, CancellationToken cancellationToken)
        {
            var conversationUpdate = request.MapToConversationHistory(accountIdTransport.AccountId);
            await convoHistoryStore.Create(conversationUpdate);
        }
    }

    public class UpdateChatHistoryRequest : INotification
    {
        public string ConversationId { get; set; }
        public string Prompt { get; set; }
        public string? UserResponse { get; set; }
        public string NodeId { get; set; }
        public bool NodeCritical { get; set; }
        public string NodeType { get; set; }

        public ConversationHistoryRow MapToConversationHistory(string accountId)
        {
            var timeStamp = DateTime.UtcNow;

            return new ConversationHistoryRow
            {
                ConversationId = ConversationId,
                Prompt = Prompt,
                UserResponse = UserResponse,
                NodeId = NodeId,
                NodeCritical = NodeCritical,
                NodeType = NodeType,
                TimeStamp = timeStamp,
                AccountId = accountId
            };
        }
    }
}