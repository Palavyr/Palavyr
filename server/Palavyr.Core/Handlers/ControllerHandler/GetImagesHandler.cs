using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetFileAssetsHandler : IRequestHandler<GetFileAssetsRequest, GetFileAssetsResponse>
    {
        private readonly IConfigurationEntityStore<FileAsset> fileAssetStore;

        public GetFileAssetsHandler(IConfigurationEntityStore<FileAsset> fileAssetStore)
        {
            this.fileAssetStore = fileAssetStore;
        }

        public async Task<GetFileAssetsResponse> Handle(GetFileAssetsRequest request, CancellationToken cancellationToken)
        {
            var fileAssets = await fileAssetStore.GetMany(request.FileIds, x => x.FileId);
            return new GetFileAssetsResponse(fileAssets);
        }
    }

    public class GetFileAssetsResponse
    {
        public GetFileAssetsResponse(List<FileAsset> response) => Response = response.ToArray();
        public FileAsset[] Response { get; set; }
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