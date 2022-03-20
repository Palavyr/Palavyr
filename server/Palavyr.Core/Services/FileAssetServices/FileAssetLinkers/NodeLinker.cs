#nullable enable
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.FileAssetServices.FileAssetLinkers
{
    public class NodeLinker : IFileAssetLinker<NodeLinker>
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IEntityStore<FileAsset> fileAssetStore;

        public NodeLinker(
            IEntityStore<ConversationNode> convoNodeStore,
            IEntityStore<FileAsset> fileAssetStore)
        {
            this.convoNodeStore = convoNodeStore;
            this.fileAssetStore = fileAssetStore;
        }

        public async Task Link(string fileId, string nodeId)
        {
            var node = await convoNodeStore.Get(nodeId, s => s.NodeId);
            var fileAsset = await fileAssetStore.Get(fileId, s => s.FileId);
            node.ImageId = fileAsset.FileId;
        }

        public async Task Unlink(string fileId, string _)
        {
            var nodes = await convoNodeStore.GetMany(fileId, s => s.ImageId);
            foreach (var node in nodes)
            {
                node.ImageId = "";
            }
        }
    }
}