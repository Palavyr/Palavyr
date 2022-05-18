using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.FileAssetServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class DeleteImagesHandler : IRequestHandler<DeleteImagesRequest, DeleteImagesResponse>
    {
        private readonly IMapToNew<FileAsset, FileAssetResource> mapper;
        private readonly IFileAssetDeleter fileAssetDeleter;

        public DeleteImagesHandler(IMapToNew<FileAsset, FileAssetResource> mapper, IFileAssetDeleter fileAssetDeleter)
        {
            this.mapper = mapper;
            this.fileAssetDeleter = fileAssetDeleter;
        }

        public async Task<DeleteImagesResponse> Handle(DeleteImagesRequest request, CancellationToken cancellationToken)
        {
            var fileAssets = await fileAssetDeleter.RemoveFiles(request.FileIds);
            var mapped = await mapper.MapMany(FilterResponsesOut(fileAssets));
            return new DeleteImagesResponse(mapped);
        }


        private IEnumerable<FileAsset> FilterResponsesOut(IEnumerable<FileAsset> fileAssets)
        {
            var barred = new[] { ResponsePrefix.Palavyr, ResponsePrefix.Preview };

            var filtered = new List<FileAsset>();
            foreach (var fileAsset in fileAssets)
            {
                var isBarred = false;
                foreach (var bar in barred)
                {
                    if (fileAsset.RiskyNameStem.StartsWith(bar))
                    {
                        isBarred = true;
                    }
                }

                if (!isBarred)
                {
                    filtered.Add(fileAsset);
                }
            }

            return filtered;
        }
    }

    public class DeleteImagesResponse
    {
        public DeleteImagesResponse(IEnumerable<FileAssetResource> response) => Response = response;
        public IEnumerable<FileAssetResource> Response { get; set; }
    }

    public class DeleteImagesRequest : IRequest<DeleteImagesResponse>
    {
        public DeleteImagesRequest(string[] fileIds)
        {
            FileIds = fileIds;
        }

        public string[] FileIds { get; set; }
    }
}