using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.FileAssetServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SaveMultipleImagesHandler : IRequestHandler<SaveMultipleImagesRequest, SaveMultipleImagesResponse>
    {
        private readonly INodeFileAssetSaver nodeFileAssetSaver;

        public SaveMultipleImagesHandler(INodeFileAssetSaver nodeFileAssetSaver)
        {
            this.nodeFileAssetSaver = nodeFileAssetSaver;
        }

        public async Task<SaveMultipleImagesResponse> Handle(SaveMultipleImagesRequest request, CancellationToken cancellationToken)
        {
            var imageFileLinks = new List<FileLink>();
            foreach (var imageFile in request.ImageFiles)
            {
                var fileLink = await nodeFileAssetSaver.SaveImage(imageFile, cancellationToken);
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