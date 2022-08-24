using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetFinishedConversationHistoryHandler : IRequestHandler<GetCompleteConversationDetailsRequest, GetCompleteConversationDetailsResponse>
    {
        private readonly IEntityStore<ConversationHistoryRow> convoHistoryRowStore;
        private readonly IMapToNew<ConversationHistoryRow, Resources.ConversationHistoryRowResource> conversationHistoryRowResourceMapper;
        private readonly ILogger<GetFinishedConversationHistoryHandler> logger;

        public GetFinishedConversationHistoryHandler(
            IEntityStore<ConversationHistoryRow> convoHistoryRowStore,
            IMapToNew<ConversationHistoryRow, Resources.ConversationHistoryRowResource> conversationHistoryRowResourceMapper,
            ILogger<GetFinishedConversationHistoryHandler> logger)
        {
            this.convoHistoryRowStore = convoHistoryRowStore;
            this.conversationHistoryRowResourceMapper = conversationHistoryRowResourceMapper;
            this.logger = logger;
        }

        public async Task<GetCompleteConversationDetailsResponse> Handle(GetCompleteConversationDetailsRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Collecting Conversation for viewing...");
            var convoRows = await convoHistoryRowStore.GetMany(request.ConversationId, s => s.ConversationId);
            convoRows.Sort((x, y) => x.Id > y.Id ? 1 : -1);

            var resources = await conversationHistoryRowResourceMapper.MapMany(convoRows, cancellationToken);
            return new GetCompleteConversationDetailsResponse(resources.ToArray());
        }
    }


    public class GetCompleteConversationDetailsResponse
    {
        public GetCompleteConversationDetailsResponse(Resources.ConversationHistoryRowResource[] response) => Response = response;
        public Resources.ConversationHistoryRowResource[] Response { get; set; }
    }

    public class GetCompleteConversationDetailsRequest : IRequest<GetCompleteConversationDetailsResponse>
    {
        public GetCompleteConversationDetailsRequest(string conversationId)
        {
            ConversationId = conversationId;
        }

        public string ConversationId { get; set; }
    }
}