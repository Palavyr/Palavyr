using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Models.Resources.Responses;
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
            var fileLink = new FileLink
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
        public SaveSingleImageResponse(FileLink[] response) => Response = response;
        public FileLink[] Response { get; set; }
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