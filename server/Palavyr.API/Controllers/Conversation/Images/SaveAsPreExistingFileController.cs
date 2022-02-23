using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Conversation.Images
{
    public class SaveAsPreExistingFileController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private const string Route = "images/pre-existing/{imageId}/{nodeId}";

        public SaveAsPreExistingFileController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task Post(
            string imageId,
            string nodeId,
            CancellationToken cancellationToken
        )
        {
            await mediator.Publish(new SaveAsPreExistingFileRequest(imageId, nodeId), cancellationToken);
        }
    }
}