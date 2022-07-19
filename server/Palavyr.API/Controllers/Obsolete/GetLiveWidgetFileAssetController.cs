using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Obsolete
{
    [Obsolete("It seems this is no longer used by the frontend")]
    public class GetLiveWidgetFileAssetController // : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private const string Route = "widget/node-file-asset/{nodeId}";

        public GetLiveWidgetFileAssetController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // [HttpGet(Route)]
        public async Task<FileAssetResource> GetFileAsset(
            [FromRoute]
            string nodeId,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(new GetLiveWidgetFileAssetRequest(), cancellationToken);
            return response.Response;
        }
    }
}