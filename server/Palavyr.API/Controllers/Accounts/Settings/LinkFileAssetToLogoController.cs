using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class LinkFileAssetToLogoController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "file-assets/link/{fileId}/logo";


        public LinkFileAssetToLogoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task Link(
            string fileId,
            CancellationToken cancellationToken
        )
        {
            await mediator.Publish(new LinkFileAssetToLogoRequest(fileId), cancellationToken);
        }
    }
}