using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.FileAssetServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class UploadNodeFileAssetsHandler : IRequestHandler<UploadFileAssetsRequest, UploadFileAssetsResponse>
    {
        private readonly IFileAssetSaver fileAssetSaver;
        private readonly IMapToNew<FileAsset, FileAssetResource> mapper;

        public UploadNodeFileAssetsHandler(INodeFileAssetSaver fileAssetSaver, IMapToNew<FileAsset, FileAssetResource> mapper)
        {
            this.fileAssetSaver = fileAssetSaver;
            this.mapper = mapper;
        }

        public async Task<UploadFileAssetsResponse> Handle(UploadFileAssetsRequest request, CancellationToken cancellationToken)
        {
            var fileAssets = new List<FileAsset>();
            foreach (var imageFile in request.ImageFiles)
            {
                var fileAsset = await fileAssetSaver.SaveFile(imageFile);
                fileAssets.Add(fileAsset);
            }

            var resources = await mapper.MapMany(fileAssets, cancellationToken);
            return new UploadFileAssetsResponse(resources);
        }
    }

    public class UploadFileAssetsResponse
    {
        public UploadFileAssetsResponse(IEnumerable<FileAssetResource> response) => Response = response;
        public IEnumerable<FileAssetResource> Response { get; set; }
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