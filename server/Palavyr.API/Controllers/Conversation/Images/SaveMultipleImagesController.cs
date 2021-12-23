using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.ImageServices;

namespace Palavyr.API.Controllers.Conversation.Images
{
    // This should be refactored and we only ever accept arrays of filelinks
    public class SaveMultipleImagesController : PalavyrBaseController
    {
        private readonly IImageSaver imageSaver;
        private const string Route = "images/save-many";

        public SaveMultipleImagesController(IImageSaver imageSaver)
        {
            this.imageSaver = imageSaver;
        }

        [HttpPost(Route)]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveMany(

            [FromForm(Name = "files")]
            List<IFormFile> imageFiles,
            CancellationToken cancellationToken)
        {
            var imageFileLinks = new List<FileLink>();
            foreach (var imageFile in imageFiles)
            {
                var fileLink = await imageSaver.SaveImage(imageFile, cancellationToken);
                imageFileLinks.Add(fileLink);
            }

            return imageFileLinks.ToArray();
        }
    }
}