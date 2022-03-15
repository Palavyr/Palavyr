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
    public class UploadFileAssetsHandler : IRequestHandler<UploadFileAssetsRequest, UploadFileAssetsResponse>
    {
        private readonly INodeFileAssetSaver nodeFileAssetSaver;
        private readonly ILinkCreator linkCreator;

        public UploadFileAssetsHandler(INodeFileAssetSaver nodeFileAssetSaver, ILinkCreator linkCreator)
        {
            this.nodeFileAssetSaver = nodeFileAssetSaver;
            this.linkCreator = linkCreator;
        }

        public async Task<UploadFileAssetsResponse> Handle(UploadFileAssetsRequest request, CancellationToken cancellationToken)
        {
            var imageFileLinks = new List<FileLink>();
            foreach (var imageFile in request.ImageFiles)
            {
                var fileAsset = await nodeFileAssetSaver.SaveFile(imageFile);
                imageFileLinks.Add(await fileAsset.ToFileLink(linkCreator));
            }

            return new UploadFileAssetsResponse(imageFileLinks.ToArray());
        }
    }

    public class UploadFileAssetsResponse
    {
        public UploadFileAssetsResponse(FileLink[] response) => Response = response;
        public FileLink[] Response { get; set; }
    }

    public class UploadFileAssetsRequest : IRequest<UploadFileAssetsResponse>
    {
        public UploadFileAssetsRequest(List<IFormFile> imageFiles)
        {
            ImageFiles = imageFiles;
        }

        public List<IFormFile> ImageFiles { get; set; }
    }
}