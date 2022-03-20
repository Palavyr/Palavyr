using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Mappers;

namespace Palavyr.API.Controllers.Attachments
{
    public class DeleteAttachmentController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "attachments";

        public DeleteAttachmentController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpDelete(Route)]
        public async Task<IEnumerable<FileAssetResource>> Delete(
            [FromBody]
            DeleteAttachmentRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}