using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class UpdateChatHistoryHandler : INotificationHandler<UpdateChatHistoryRequest>
    {
        private readonly ConvoContext convoContext;
        private readonly ILogger<UpdateChatHistoryHandler> logger;
        private readonly IAccountIdTransport accountIdTransport;

        public UpdateChatHistoryHandler(ConvoContext convoContext, ILogger<UpdateChatHistoryHandler> logger, IAccountIdTransport accountIdTransport)
        {
            this.convoContext = convoContext;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task Handle(UpdateChatHistoryRequest request, CancellationToken cancellationToken)
        {
            var conversationUpdate = request.MapToConversationHistory(accountIdTransport.AccountId);
            convoContext.ConversationHistories.Add(conversationUpdate);
            await convoContext.SaveChangesAsync();
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

        public ConversationHistory MapToConversationHistory(string accountId)
        {
            var timeStamp = DateTime.UtcNow;

            return new ConversationHistory
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