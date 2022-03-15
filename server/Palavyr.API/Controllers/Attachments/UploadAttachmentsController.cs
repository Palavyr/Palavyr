using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Attachments
{
    public class UploadAttachmentsController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "attachments/{intentId}/upload";


        public UploadAttachmentsController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        [ActionName("Decode")]
        public async Task<IEnumerable<FileAssetResource>> SaveMany(
            [FromRoute]
            string intentId,
            [FromForm(Name = "files")]
            IList<IFormFile> attachmentFiles,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(new UploadAttachmentsRequest(intentId, attachmentFiles), cancellationToken);
            return response.Response;
        }
    }
}