using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.ImageServices;

namespace Palavyr.Core.Handlers
{
    public class SaveMultipleImagesHandler : IRequestHandler<SaveMultipleImagesRequest, SaveMultipleImagesResponse>
    {
        private readonly IImageSaver imageSaver;

        public SaveMultipleImagesHandler(IImageSaver imageSaver)
        {
            this.imageSaver = imageSaver;
        }

        public async Task<SaveMultipleImagesResponse> Handle(SaveMultipleImagesRequest request, CancellationToken cancellationToken)
        {
            var imageFileLinks = new List<FileLink>();
            foreach (var imageFile in request.ImageFiles)
            {
                var fileLink = await imageSaver.SaveImage(imageFile, cancellationToken);
                imageFileLinks.Add(fileLink);
            }

            return new SaveMultipleImagesResponse(imageFileLinks.ToArray());
        }
    }

    public class SaveMultipleImagesResponse
    {
        public SaveMultipleImagesResponse(FileLink[] response) => Response = response;
        public FileLink[] Response { get; set; }
    }

    public class SaveMultipleImagesRequest : IRequest<SaveMultipleImagesResponse>
    {
        public SaveMultipleImagesRequest(List<IFormFile> imageFiles)
        {
            ImageFiles = imageFiles;
        }

        public List<IFormFile> ImageFiles { get; set; }
    }
}