using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Conversation
{
    public class GetConversationTreeErrors : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "configure-conversations/tree-errors";

        public GetConversationTreeErrors(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task<TreeErrorsResponse> Get(
            [FromBody]
            GetMissingNodesRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}