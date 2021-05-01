using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Responses;

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
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            CancellationToken cancellationToken)
        {
            var attachmentFileLinks = await attachmentRetriever.RetrieveAttachments(accountId, areaId, cancellationToken);
            return attachmentFileLinks;
        }
    }
}