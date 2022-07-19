using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;
using ConversationHistoryRowResource = Palavyr.Core.Resources.ConversationHistoryRowResource;

namespace Palavyr.API.Controllers.Enquiries
{
    public class GetCompleteConversationDetailsController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "enquiries/review/{conversationId}";


        public GetCompleteConversationDetailsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<IEnumerable<ConversationHistoryRowResource>> Get([FromRoute] string conversationId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetCompleteConversationDetailsRequest(conversationId), cancellationToken);
            return response.Response;
        }
    }
}