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
    public class GetConversationByAreaIdController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "configure-conversations/{areaId}";

        public GetConversationByAreaIdController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<List<ConversationNode>> Get([FromRoute] string areaId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetConversationRequest(areaId), cancellationToken);
            return response.Response;
        }
    }
}