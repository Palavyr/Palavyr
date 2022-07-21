using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class UpdateChatHistoryHandler : INotificationHandler<UpdateChatHistoryRequest>
    {
        private readonly IEntityStore<ConversationHistoryRow> convoHistoryStore;
        private readonly ILogger<UpdateChatHistoryHandler> logger;

        private readonly IMapToNew<ConversationHistoryRowResource, ConversationHistoryRow> mapper;

        public UpdateChatHistoryHandler(
            IEntityStore<ConversationHistoryRow> convoHistoryStore,
            ILogger<UpdateChatHistoryHandler> logger,
            IMapToNew<ConversationHistoryRowResource, ConversationHistoryRow> mapper)
        {
            this.convoHistoryStore = convoHistoryStore;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task Handle(UpdateChatHistoryRequest request, CancellationToken cancellationToken)
        {
            var conversationUpdate = await mapper.Map(request.Resource, cancellationToken);
            await convoHistoryStore.Create(conversationUpdate);
        }
    }

    public class UpdateChatHistoryRequest : INotification
    {
        public const string Route = "widget/conversation";

        public ConversationHistoryRowResource Resource { get; }

        public UpdateChatHistoryRequest(ConversationHistoryRowResource resource)
        {
            Resource = resource;
        }
    }
}