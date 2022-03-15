using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Conversation.FileAssets
{
    public class UploadFileAssetsController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private const string Route = "file-assets/upload";

        public UploadFileAssetsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveMany(

            [FromForm(Name = "files")]
            List<IFormFile> imageFiles,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new UploadFileAssetsRequest(imageFiles), cancellationToken);
            return response.Response;
        }
    }
}