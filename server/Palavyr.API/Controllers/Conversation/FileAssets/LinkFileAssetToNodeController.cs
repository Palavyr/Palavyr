using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Conversation.FileAssets
{
    public class LinkFileAssetToNode : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private const string Route = "file-assets/link/{fileId}/node/{nodeId}";

        public LinkFileAssetToNode(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task Post(
            string fileId,
            string nodeId,
            CancellationToken cancellationToken
        )
        {
            await mediator.Publish(new LinkFileAttachmentToNodeRequest(fileId, nodeId), cancellationToken);
        }
    }
}