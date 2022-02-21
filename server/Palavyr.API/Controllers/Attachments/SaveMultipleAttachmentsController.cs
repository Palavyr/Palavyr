using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Attachments
{
    public class SaveMultipleAttachmentsController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "attachments/{intentId}/save-many";


        public SaveMultipleAttachmentsController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveMany(
            [FromRoute]
            string intentId,
            [FromForm(Name = "files")]
            IList<IFormFile> attachmentFiles,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(new SaveMultipleAttachmentsRequest(intentId, attachmentFiles), cancellationToken);
            return response.Response;
        }
    }
}