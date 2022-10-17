using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Response
{
    public class GetResponsePreviewController : PalavyrBaseController
    {
        private readonly IMediator mediator;


        public GetResponsePreviewController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(GetResponsePreviewRequest.Route)]
        public async Task<FileAssetResource> GetConfigurationPreview(string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetResponsePreviewRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}