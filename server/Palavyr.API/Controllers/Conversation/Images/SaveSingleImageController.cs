using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.ImageServices;

namespace Palavyr.API.Controllers.Conversation.Images
{
    public class SaveSingleImageController : PalavyrBaseController
    {
        private readonly IImageSaver imageSaver;

        private const string Route = "images/save-one";

        public SaveSingleImageController(IImageSaver imageSaver)
        {
            this.imageSaver = imageSaver;
        }

        [HttpPost(Route)]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveSingle(
            [FromHeader]
            string accountId,
            [FromForm(Name = "files")]
            IFormFile imageFile,
            CancellationToken cancellationToken)
        {
            var fileLink = await imageSaver.SaveImage(accountId, imageFile, cancellationToken);
            return new[] {fileLink};
        }
    }
}