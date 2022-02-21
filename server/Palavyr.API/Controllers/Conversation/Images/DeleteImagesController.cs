using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.ImageServices;

namespace Palavyr.API.Controllers.Conversation.Images
{
    public class DeleteImagesController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private readonly IImageRemover imageRemover;
        private readonly GuidFinder guidFinder;
        private const string Route = "images";

        public DeleteImagesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpDelete(Route)]
        public async Task<FileLink[]> DeleteImageById(
            [FromQuery]
            string imageIds,
            CancellationToken cancellationToken
        )
        {
            var ids = imageIds?.Split(",") ?? new string[] { };
            var response = await mediator.Send(new DeleteImagesRequest(ids), cancellationToken);
            return response.Response;
        }
    }
}