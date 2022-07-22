using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
// using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Conversation
{
    public class ModifyConversationNodeController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "configure-conversations/{intentId}/nodes/{nodeId}";


        public ModifyConversationNodeController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<IEnumerable<ConversationDesignerNodeResource>> Modify(
            [FromRoute]
            string nodeId,
            [FromRoute]
            string intentId,
            [FromBody]
            ConversationDesignerNodeResource newNode,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new ModifyConversationNodeRequest(nodeId, intentId, newNode), cancellationToken);
            return response.Response;
        }
    }
}