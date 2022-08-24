using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Attachments
{
    public class GetAttachmentsController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "attachments/{intentId}";

        public GetAttachmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<IEnumerable<FileAssetResource>> Get(
            [FromRoute]
            string intentId,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetAttachmentsRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}