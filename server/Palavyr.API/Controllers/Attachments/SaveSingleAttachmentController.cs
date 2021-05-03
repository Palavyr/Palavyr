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

        public SaveSingleAttachmentController(
            IAttachmentSaver attachmentSaver
        )
        {
            this.attachmentSaver = attachmentSaver;
        }

        [HttpPost("attachments/{areaId}/save-one")]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveSingle(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromForm(Name = "files")] IFormFile attachmentFile)
        {
            var fileLink = await attachmentSaver.SaveAttachment(accountId, areaId, attachmentFile);
            return new[] {fileLink};
        }
    }
}