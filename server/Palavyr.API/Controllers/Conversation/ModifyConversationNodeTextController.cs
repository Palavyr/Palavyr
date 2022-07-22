using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Conversation
{
    public class ModifyConversationNodeTextController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "configure-conversations/nodes/text";

        public ModifyConversationNodeTextController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<ConversationDesignerNodeResource> Modify(
            [FromBody]
            ModifyConversationNodeTextRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}