using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetLiveWidgetFileAssetHandler : IRequestHandler<GetLiveWidgetFileAssetRequest, GetLiveWidgetFileAssetResponse>
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IEntityStore<FileAsset> fileAssetStore;
        private readonly IMapToNew<FileAsset, FileAssetResource> mapper;

        public GetLiveWidgetFileAssetHandler(
            IEntityStore<ConversationNode> convoNodeStore,
            IEntityStore<FileAsset> fileAssetStore,
            IMapToNew<FileAsset, FileAssetResource> mapper)
        {
            this.convoNodeStore = convoNodeStore;
            this.fileAssetStore = fileAssetStore;
            this.mapper = mapper;
        }

        public async Task<GetLiveWidgetFileAssetResponse> Handle(GetLiveWidgetFileAssetRequest request, CancellationToken cancellationToken)
        {
            var convoNode = await convoNodeStore.Get(request.NodeId, x => x.NodeId);
            var fileAsset = await fileAssetStore.GetOrNull(convoNode.FileId, x => x.FileId);

            if (fileAsset is null)
            {
                return new GetLiveWidgetFileAssetResponse(new FileAssetResource());
            }

            if (string.IsNullOrEmpty(fileAsset.LocationKey))
            {
                throw new DomainException("Failed to set the file key for this image.");
            }

            var resource = await mapper.Map(fileAsset);
            return new GetLiveWidgetFileAssetResponse(resource);
        }
    }

    public class GetLiveWidgetFileAssetResponse
    {
        public GetLiveWidgetFileAssetResponse(FileAssetResource response) => Response = response;
        public FileAssetResource Response { get; set; }
    }

    public class GetLiveWidgetFileAssetRequest : IRequest<GetLiveWidgetFileAssetResponse>
    {
        public string NodeId { get; set; }
    }
}