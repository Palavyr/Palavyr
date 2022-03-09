using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SaveAsPreExistingFileHandler : INotificationHandler<SaveAsPreExistingFileRequest>
    {
        private readonly IConfigurationEntityStore<FileAsset> fileAssetStore;
        private readonly IConfigurationEntityStore<ConversationNode> convoNodeStore;

        public SaveAsPreExistingFileHandler(IConfigurationEntityStore<FileAsset> fileAssetStore, IConfigurationEntityStore<ConversationNode> convoNodeStore)
        {
            this.fileAssetStore = fileAssetStore;
            this.convoNodeStore = convoNodeStore;
        }

        public async Task Handle(SaveAsPreExistingFileRequest request, CancellationToken cancellationToken)
        {
            // asserts this image exists

            var fileAsset = await fileAssetStore.Get(request.FileId, s => s.FileId);
            var convoNode = await convoNodeStore.Get(request.NodeId, s => s.NodeId);

            if (convoNode != null)
            {
                convoNode.ImageId = fileAsset.FileId;
            }
        }
    }


    public class SaveAsPreExistingFileRequest : INotification
    {
        public SaveAsPreExistingFileRequest(string fileId, string nodeId)
        {
            FileId = fileId;
            NodeId = nodeId;
        }

        public string FileId { get; set; }
        public string NodeId { get; set; }
    }
}