using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Conversation
{
    public class ModifyConversationController : PalavyrBaseController
    {
        public const string Route = "configure-conversations";
        private readonly IMediator mediator;

        public ModifyConversationController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<IEnumerable<ConversationDesignerNodeResource>> Modify(
            [FromBody]
            ModifyConversationRequest request,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}