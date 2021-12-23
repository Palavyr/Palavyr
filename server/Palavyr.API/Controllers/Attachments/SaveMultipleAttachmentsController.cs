using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AttachmentServices;

namespace Palavyr.API.Controllers.Attachments
{
    public class SaveMultipleAttachmentsController : PalavyrBaseController
    {
        private readonly IAttachmentSaver attachmentSaver;
        private readonly IAttachmentRetriever attachmentRetriever;

        public SaveMultipleAttachmentsController(
            IAttachmentSaver attachmentSaver,
            IAttachmentRetriever attachmentRetriever
        )
        {
            this.attachmentSaver = attachmentSaver;
            this.attachmentRetriever = attachmentRetriever;
        }

        [HttpPost("attachments/{areaId}/save-many")]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveMany(
            [FromRoute] string areaId,
            [FromForm(Name = "files")] IList<IFormFile> attachmentFiles,
            CancellationToken cancellationToken
        )
        {
            foreach (var attachmentFile in attachmentFiles)
            {
                await attachmentSaver.SaveAttachment(areaId, attachmentFile);
            }
            var attachmentFileLinks = await attachmentRetriever.RetrieveAttachmentLinks(areaId, cancellationToken);
            return attachmentFileLinks;
        }
    }
}