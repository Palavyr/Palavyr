using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.FileAssetServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SaveMultipleImagesHandler : IRequestHandler<SaveMultipleImagesRequest, SaveMultipleImagesResponse>
    {
        private readonly INodeFileAssetSaver nodeFileAssetSaver;
        private readonly ILinkCreator linkCreator;

        public SaveMultipleImagesHandler(INodeFileAssetSaver nodeFileAssetSaver, ILinkCreator linkCreator)
        {
            this.nodeFileAssetSaver = nodeFileAssetSaver;
            this.linkCreator = linkCreator;
        }

        public async Task<SaveMultipleImagesResponse> Handle(SaveMultipleImagesRequest request, CancellationToken cancellationToken)
        {
            var imageFileLinks = new List<FileLink>();
            foreach (var imageFile in request.ImageFiles)
            {
                var fileAsset = await nodeFileAssetSaver.SaveFile(imageFile);
                imageFileLinks.Add(await fileAsset.ToFileLink(linkCreator));
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