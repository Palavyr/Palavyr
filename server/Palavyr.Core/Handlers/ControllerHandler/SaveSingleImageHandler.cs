using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Resources.Responses;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.FileAssetServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SaveSingleImageHandler : IRequestHandler<SaveSingleImageRequest, SaveSingleImageResponse>
    {
        private readonly INodeFileAssetSaver nodeFileAssetSaver;
        private readonly ILinkCreator linkCreator;

        public SaveSingleImageHandler(INodeFileAssetSaver nodeFileAssetSaver, ILinkCreator linkCreator)
        {
            this.nodeFileAssetSaver = nodeFileAssetSaver;
            this.linkCreator = linkCreator;
        }

        public async Task<SaveSingleImageResponse> Handle(SaveSingleImageRequest request, CancellationToken cancellationToken)
        {
            var fileAsset = await nodeFileAssetSaver.SaveFile(request.ImageFile);
            var fileLink = new FileLinkResource
            {
                Link = await linkCreator.CreateLink(fileAsset.FileId),
                FileId = fileAsset.FileId,
                FileName = fileAsset.RiskyNameWithExtension
            };
            return new SaveSingleImageResponse(new[] { fileLink });
        }
    }

    public class SaveSingleImageResponse
    {
        public SaveSingleImageResponse(FileLinkResource[] response) => Response = response;
        public FileLinkResource[] Response { get; set; }
    }

    public class SaveSingleImageRequest : IRequest<SaveSingleImageResponse>
    {
        public IFormFile ImageFile { get; set; }

        public SaveSingleImageRequest(IFormFile imageFile)
        {
            ImageFile = imageFile;
        }
    }
}