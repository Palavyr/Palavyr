using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Conversation
{
    public class ModifyConversationNodeController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "configure-conversations/{areaId}/nodes/{nodeId}";


        public ModifyConversationNodeController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<List<ConversationNode>> Modify(
            [FromRoute]
            string nodeId,
            [FromRoute]
            string areaId,
            [FromBody]
            ConversationNode newNode,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new ModifyConversationNodeRequest(nodeId, areaId, newNode), cancellationToken);
            return response.Response;
        }
    }
}