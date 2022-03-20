using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.WidgetLive
{
    public class GetLiveWidgetImageUrlController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private const string Route = "widget/node-image/{nodeId}";

        public GetLiveWidgetImageUrlController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<string> GetImageUrl(
            [FromRoute]
            string nodeId,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(new GetLiveWidgetImageUrlRequest(), cancellationToken);
            return response.Response;
        }
    }
}