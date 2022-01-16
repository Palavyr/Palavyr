using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.ConversationServices;

namespace Palavyr.Core.Handlers
{
    public class ModifyCompletedConversationsHandler : IRequestHandler<ModifyCompletedConversationsRequest, ModifyCompletedConversationsResponse>
    {
        private readonly ICompletedConversationModifier completedConversationModifier;
        private readonly ILogger<ModifyCompletedConversationsHandler> logger;

        public ModifyCompletedConversationsHandler(
            ICompletedConversationModifier completedConversationModifier,
            ILogger<ModifyCompletedConversationsHandler> logger)
        {
            this.completedConversationModifier = completedConversationModifier;
            this.logger = logger;
        }

        public async Task<ModifyCompletedConversationsResponse> Handle(ModifyCompletedConversationsRequest request, CancellationToken cancellationToken)
        {
            var modifiedCompletedConversation = await completedConversationModifier.ModifyCompletedConversation(request.ConversationId);
            return new ModifyCompletedConversationsResponse(modifiedCompletedConversation);
        }
    }

    public class ModifyCompletedConversationsResponse
    {
        public ModifyCompletedConversationsResponse(Enquiry[] response) => Response = response;
        public Enquiry[] Response { get; set; }
    }

    public class ModifyCompletedConversationsRequest : IRequest<ModifyCompletedConversationsResponse>
    {
        public ModifyCompletedConversationsRequest(string conversationId)
        {
            ConversationId = conversationId;
        }

        public string ConversationId { get; set; }
    }
}