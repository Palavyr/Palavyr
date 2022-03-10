using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetLiveWidgetImageUrlHandler : IRequestHandler<GetLiveWidgetImageUrlRequest, GetLiveWidgetImageUrlResponse>
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IEntityStore<FileAsset> fileAssetStore;
        private readonly ILinkCreator linkCreator;

        public GetLiveWidgetImageUrlHandler(
            IEntityStore<ConversationNode> convoNodeStore,
            IEntityStore<FileAsset> fileAssetStore,
            ILinkCreator linkCreator)
        {
            this.convoNodeStore = convoNodeStore;
            this.fileAssetStore = fileAssetStore;
            this.linkCreator = linkCreator;
        }

        public async Task<GetLiveWidgetImageUrlResponse> Handle(GetLiveWidgetImageUrlRequest request, CancellationToken cancellationToken)
        {
            var convoNode = await convoNodeStore.Get(request.NodeId, x => x.NodeId);
            var imageFileAsset = await fileAssetStore.Get(convoNode.ImageId, x => x.FileId);
            if (imageFileAsset.LocationKey == null)
            {
                throw new DomainException("Failed to set the file key for this image.");
            }

            var link = await linkCreator.CreateLink(imageFileAsset.FileId);

            return new GetLiveWidgetImageUrlResponse(link);
        }
    }

    public class GetLiveWidgetImageUrlResponse
    {
        public GetLiveWidgetImageUrlResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetLiveWidgetImageUrlRequest : IRequest<GetLiveWidgetImageUrlResponse>
    {
        public string NodeId { get; set; }
    }
}