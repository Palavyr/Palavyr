using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Conversation.Images
{
    public class SaveSingleImageController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        private const string Route = "images/save-one";

        public SaveSingleImageController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveSingle(
            [FromForm(Name = "files")]
            IFormFile imageFile,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new SaveSingleImageRequest(imageFile), cancellationToken);
            return response.Response;
        }
    }
}