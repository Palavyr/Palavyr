using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Conversation
{
    public class GetIsMultiOptionTypeController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "configure-conversations/check-multi-option/{nodeType}";
        

        public GetIsMultiOptionTypeController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<bool> Get(string nodeType, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetIsMultiOptionTypeRequest(nodeType), cancellationToken);
            return response.Response;
        }
    }
}