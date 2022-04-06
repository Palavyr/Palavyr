using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.ConversationServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class MarkEnquiryAsSeenHandler : INotificationHandler<MarkEnquiryAsSeenRequest>
    {
        private readonly ICompletedConversationModifier completedConversationModifier;
        private readonly ILogger<MarkEnquiryAsSeenHandler> logger;

        public MarkEnquiryAsSeenHandler(
            ICompletedConversationModifier completedConversationModifier,
            ILogger<MarkEnquiryAsSeenHandler> logger)
        {
            this.completedConversationModifier = completedConversationModifier;
            this.logger = logger;
        }

        public async Task Handle(MarkEnquiryAsSeenRequest request, CancellationToken cancellationToken)
        {
            await completedConversationModifier.MarkAsSeen(request.Updates);
        }
    }

    public class MarkAsSeenUpdate
    {
        public string ConversationId { get; set; }
        public bool Seen { get; set; }
    }

    public class MarkEnquiryAsSeenRequest : INotification
    {
        public List<MarkAsSeenUpdate> Updates { get; set; }
    }
}