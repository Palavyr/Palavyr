using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AttachmentServices;

namespace Palavyr.API.Controllers.Attachments
{
    public class GetAttachmentsController : PalavyrBaseController
    {
        private readonly IAttachmentRetriever attachmentRetriever;

        public GetAttachmentsController(
            IAttachmentRetriever attachmentRetriever
        )
        {
            this.attachmentRetriever = attachmentRetriever;
        }

        [HttpGet("attachments/{areaId}")]
        public async Task<FileLink[]> Get(
            [FromRoute] string areaId,
            CancellationToken cancellationToken)
        {
            var attachmentFileLinks = await attachmentRetriever.RetrieveAttachmentLinks(areaId, cancellationToken);
            return attachmentFileLinks;
        }
    }
}