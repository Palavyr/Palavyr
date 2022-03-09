using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Services.FileAssetServices.FileAssetLinkers
{
    public class FileAssetLinker : IFileAssetLinker<FileAssetLinker>
    {
        private readonly IConfigurationEntityStore<ConversationNode> convoNodeStore;
        private readonly IConfigurationEntityStore<FileAsset> fileAssetStore;

        public FileAssetLinker(
            IConfigurationEntityStore<ConversationNode> convoNodeStore,
            IConfigurationEntityStore<FileAsset> fileAssetStore)
        {
            this.convoNodeStore = convoNodeStore;
            this.fileAssetStore = fileAssetStore;
        }

        public async Task LinkToNode(string fileId, string nodeId)
        {
            var node = await convoNodeStore.Get(nodeId, s => s.NodeId);
            var fileAsset = await fileAssetStore.Get(fileId, s => s.FileId);
            node.ImageId = fileAsset.FileId;
        }

        public async Task UnLinkFromNode(string fileId, string nodeId)
        {
            var node = await convoNodeStore.Get(nodeId, s => s.NodeId);
            node.ImageId = null;
        }

        public Task LinkToAccount(string fileId)
        {
            throw new System.NotImplementedException();
        }

        public Task UnlinkFromAccount(string fileId)
        {
            throw new System.NotImplementedException();
        }

        public async Task LinkToIntent(string fileId, string intentId)
        {
            throw new System.NotImplementedException();
        }

        public Task UnLinkFromIntent(string fileId, string intentId)
        {
            throw new System.NotImplementedException();
        }

    }
}