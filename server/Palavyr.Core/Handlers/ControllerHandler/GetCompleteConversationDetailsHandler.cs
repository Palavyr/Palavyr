using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetCompleteConversationDetailsHandler : IRequestHandler<GetCompleteConversationDetailsRequest, GetCompleteConversationDetailsResponse>
    {
        private readonly IEntityStore<ConversationHistoryRow> convoHistoryRowStore;
        private readonly ILogger<GetCompleteConversationDetailsHandler> logger;

        public GetCompleteConversationDetailsHandler(IEntityStore<ConversationHistoryRow> convoHistoryRowStore, ILogger<GetCompleteConversationDetailsHandler> logger)
        {
            this.convoHistoryRowStore = convoHistoryRowStore;
            this.logger = logger;
        }

        public async Task<GetCompleteConversationDetailsResponse> Handle(GetCompleteConversationDetailsRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Collecting Conversation for viewing...");
            var convoRows = await convoHistoryRowStore.GetMany(request.ConversationId, s => s.ConversationId);
            convoRows.Sort((x, y) => x.Id > y.Id ? 1 : -1);

            var resources = new List<ConversationRowsResource>();
            foreach (var row in convoRows)
            {
                var newResource = new ConversationRowsResource
                {
                    ConversationId = row.ConversationId,
                    Prompt = row.Prompt,
                    UserResponse = row.UserResponse,
                    NodeId = row.NodeId,
                    NodeCritical = row.NodeCritical,
                    NodeType = row.NodeType,
                    TimeStamp = row.TimeStamp.ToString(),
                    AccountId = row.AccountId
                };
                resources.Add(newResource);
            }

            return new GetCompleteConversationDetailsResponse(resources.ToArray());
        }
    }

    public class ConversationRowsResource
    {
        public string ConversationId { get; set; }
        public string Prompt { get; set; }
        public string UserResponse { get; set; }
        public string NodeId { get; set; }
        public bool NodeCritical { get; set; }
        public string NodeType { get; set; }
        public string TimeStamp { get; set; }
        public string AccountId { get; set; }
    }

    public class GetCompleteConversationDetailsResponse
    {
        public GetCompleteConversationDetailsResponse(ConversationRowsResource[] response) => Response = response;
        public ConversationRowsResource[] Response { get; set; }
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