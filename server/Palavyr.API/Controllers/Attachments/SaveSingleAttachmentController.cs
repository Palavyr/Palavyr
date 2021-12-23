using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AttachmentServices;

namespace Palavyr.API.Controllers.Attachments
{
    public class SaveSingleAttachmentController : PalavyrBaseController
    {
        private readonly IAttachmentSaver attachmentSaver;
        private readonly IAttachmentRetriever attachmentRetriever;

        public SaveSingleAttachmentController(
            IAttachmentSaver attachmentSaver,
            IAttachmentRetriever attachmentRetriever
        )
        {
            this.attachmentSaver = attachmentSaver;
            this.attachmentRetriever = attachmentRetriever;
        }

        [HttpPost("attachments/{areaId}/save-one")]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveSingle(
            [FromRoute] string areaId,
            [FromForm(Name = "files")] IFormFile attachmentFile,
            CancellationToken cancellationToken)
        {
            await attachmentSaver.SaveAttachment(areaId, attachmentFile);
            var attachmentFileLinks = await attachmentRetriever.RetrieveAttachmentLinks(areaId, cancellationToken);
            return attachmentFileLinks;
        }
    }
}