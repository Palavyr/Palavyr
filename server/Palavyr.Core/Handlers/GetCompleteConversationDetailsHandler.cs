using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Conversation.Schemas;

namespace Palavyr.Core.Handlers
{
    public class GetCompleteConversationDetailsHandler : IRequestHandler<GetCompleteConversationDetailsRequest, GetCompleteConversationDetailsResponse>
    {
        private readonly ConvoContext convoContext;
        private readonly ILogger<GetCompleteConversationDetailsHandler> logger;

        public GetCompleteConversationDetailsHandler(ConvoContext convoContext, ILogger<GetCompleteConversationDetailsHandler> logger)
        {
            this.convoContext = convoContext;
            this.logger = logger;
        }

        public async Task<GetCompleteConversationDetailsResponse> Handle(GetCompleteConversationDetailsRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Collecting Conversation for viewing...");
            var convoRows = await convoContext
                .ConversationHistories
                .Where(row => row.ConversationId == request.ConversationId)
                .ToListAsync(cancellationToken);

            convoRows.Sort((x, y) => x.Id > y.Id ? 1 : -1);
            return new GetCompleteConversationDetailsResponse(convoRows.ToArray());
        }
    }

    public class GetCompleteConversationDetailsResponse
    {
        public GetCompleteConversationDetailsResponse(ConversationHistory[] response) => Response = response;
        public ConversationHistory[] Response { get; set; }
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