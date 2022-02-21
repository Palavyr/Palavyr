using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Conversation
{
    public class GetIsTerminalTypeController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private ILogger<GetIsTerminalTypeController> logger;
        public const string Route = "configure-conversations/check-terminal/{nodeType}";

        public GetIsTerminalTypeController(IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<bool> Get(string nodeType, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetIsTerminalTypeRequest(nodeType), cancellationToken);
            return response.Response;
        }
    }
}