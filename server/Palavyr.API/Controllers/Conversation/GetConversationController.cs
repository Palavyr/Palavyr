using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Conversation
{
    public class GetConversationByIntentIdController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "configure-conversations/{intentId}";

        public GetConversationByIntentIdController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<IEnumerable<ConversationNodeResource>> Get([FromRoute] string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetConversationRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}