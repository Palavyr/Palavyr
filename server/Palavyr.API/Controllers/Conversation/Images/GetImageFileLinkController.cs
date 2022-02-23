using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Conversation.Images
{
    public class GetImageFileLinkController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "images/link";


        public GetImageFileLinkController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task<string> Get([FromBody] GetImageFileLinkRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}