using System;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
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
            node.FileId = fileAsset.FileId;
        }

        public async Task Unlink(string fileId, string _)
        {
            if (fileId == null) throw new ArgumentNullException(nameof(fileId));
            var nodes = await convoNodeStore.GetMany(fileId, s => s.FileId);
            foreach (var node in nodes)
            {
                node.FileId = string.Empty;
            }
        }
    }
}