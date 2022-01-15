using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Conversation.Images
{
    // This should be refactored and we only ever accept arrays of filelinks
    public class SaveMultipleImagesController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private const string Route = "images/save-many";

        public SaveMultipleImagesController(IMediator mediator)
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
            var response = await mediator.Send(new SaveMultipleImagesRequest(imageFiles), cancellationToken);
            return response.Response;
        }
    }
}