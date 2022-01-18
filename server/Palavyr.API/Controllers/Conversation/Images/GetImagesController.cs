using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Conversation.Images
{
    public class GetImagesController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private const string Route = "images";

        public GetImagesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<FileLink[]> GetImages(
            [FromQuery]
            string? imageIds,
            CancellationToken cancellationToken) // should be comma separated
        {
            var ids = imageIds?.Split(',') ?? new string[] { };
            var response = await mediator.Send(new GetImagesRequest(ids), cancellationToken);
            return response.Response;
        }
    }
}