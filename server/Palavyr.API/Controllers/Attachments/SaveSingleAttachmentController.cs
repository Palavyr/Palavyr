using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Attachments
{
    public class SaveSingleAttachmentController : PalavyrBaseController
    {
        public const string Route = "attachments/{intentId}/save-one";
        private readonly IMediator mediator;

        public SaveSingleAttachmentController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveSingle(
            [FromRoute]
            string intentId,
            [FromForm(Name = "files")]
            IFormFile attachmentFile,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new SaveSingleAttachmentRequest(intentId, attachmentFile), cancellationToken);
            return response.Response;
        }
    }
}