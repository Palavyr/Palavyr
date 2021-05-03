using System.Collections.Generic;
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
     
        public SaveMultipleAttachmentsController(
            IAttachmentSaver attachmentSaver
        )
        {
            this.attachmentSaver = attachmentSaver;
        }

        [HttpPost("attachments/{areaId}/save-many")]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveMany(
            [FromRoute] string areaId,
            [FromHeader] string accountId,
            [FromForm(Name = "files")] IList<IFormFile> attachmentFiles
        )
        {
            var fileLinks = new List<FileLink>();
            foreach (var attachmentFile in attachmentFiles)
            {
                var fileLink = await attachmentSaver.SaveAttachment(accountId, areaId, attachmentFile);
                fileLinks.Add(fileLink);
            }

            return fileLinks.ToArray();
        }
    }
}