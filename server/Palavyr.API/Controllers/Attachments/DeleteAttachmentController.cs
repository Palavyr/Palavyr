using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Attachments
{
    public class DeleteAttachmentController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "attachments/file-link";

        public DeleteAttachmentController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpDelete(Route)]
        public async Task<FileLink[]> Delete(
            [FromBody]
            DeleteAttachmentRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}