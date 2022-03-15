using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Conversation.FileAssets
{
    public class LinkFileAssetToIntent : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private const string Route = "file-assets/link/{fileId}/intent/{intentId}";

        public LinkFileAssetToIntent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task Post(
            string fileId,
            string intentId,
            CancellationToken cancellationToken
        )
        {
            await mediator.Publish(new LinkFileAssetToIntentRequest(fileId, intentId), cancellationToken);
        }
    }
}