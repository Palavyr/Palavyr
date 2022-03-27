using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetFileAssetsHandler : IRequestHandler<GetFileAssetsRequest, GetFileAssetsResponse>
    {
        private readonly IEntityStore<FileAsset> fileAssetStore;
        private readonly IMapToNew<FileAsset, FileAssetResource> mapper;

        public GetFileAssetsHandler(IEntityStore<FileAsset> fileAssetStore, IMapToNew<FileAsset, FileAssetResource> mapper)
        {
            this.fileAssetStore = fileAssetStore;
            this.mapper = mapper;
        }

        public async Task<GetFileAssetsResponse> Handle(GetFileAssetsRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<FileAsset> fileAssets;
            if (request.FileIds.Length == 0)
            {
                fileAssets = await fileAssetStore.GetAll();
            }
            else
            {
                fileAssets = await fileAssetStore.GetMany(request.FileIds, x => x.FileId);
            }

            var filteredAssets = FilterResponsesOut(fileAssets);
            var resources = await mapper.MapMany(filteredAssets);
            return new GetFileAssetsResponse(resources);
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

    public class GetFileAssetsResponse
    {
        public GetFileAssetsResponse(IEnumerable<FileAssetResource> response) => Response = response;
        public IEnumerable<FileAssetResource> Response { get; set; }
    }

    public class GetFileAssetsRequest : IRequest<GetFileAssetsResponse>
    {
        public GetFileAssetsRequest(string[] fileIds)
        {
            FileIds = fileIds;
        }

        public string[] FileIds { get; set; }
    }
}