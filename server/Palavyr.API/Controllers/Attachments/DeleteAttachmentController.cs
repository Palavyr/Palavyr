using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AttachmentServices;

namespace Palavyr.API.Controllers.Attachments
{
    public class DeleteAttachmentController : PalavyrBaseController
    {
        private ILogger<DeleteAttachmentController> logger;

        private readonly IAttachmentDeleter attachmentDeleter;
        private readonly IAttachmentRetriever attachmentRetriever;

        public DeleteAttachmentController(
            ILogger<DeleteAttachmentController> logger,
            IAttachmentDeleter attachmentDeleter,
            IAttachmentRetriever attachmentRetriever
        )
        {
            this.logger = logger;
            this.attachmentDeleter = attachmentDeleter;
            this.attachmentRetriever = attachmentRetriever;
        }

        [HttpDelete("attachments/{areaId}/file-link")]
        public async Task<FileLink[]> Delete(
            [FromRoute] string areaId,
            [FromBody] DeleteAttachmentRequest request,
            CancellationToken cancellationToken)
        {
            await attachmentDeleter.DeleteAttachment(request.FileId, cancellationToken);
            
            // this is currently pretty slow -- we should be caching the presigned URLs and only refreshing them once they are invalid.
            // this will always refresh the pre-signed URLs (not a huge problem, but still).
            var attachmentFileLinks = await attachmentRetriever.RetrieveAttachmentLinks(areaId, cancellationToken);
            return attachmentFileLinks;
        }
    }

    public class DeleteAttachmentRequest
    {
        [FromBody]
        public string FileId { get; set; }
    }
}