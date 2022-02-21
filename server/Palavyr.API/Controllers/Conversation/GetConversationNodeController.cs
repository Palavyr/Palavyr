using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Conversation
{
    public class GetConversationNodeController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "configure-conversations/nodes/{nodeId}";


        public GetConversationNodeController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<ConversationNode> Get([FromRoute] string nodeId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetConversationNodeRequest(nodeId), cancellationToken);
            return response.Response;
        }
    }
}