using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.FileAssetServices
{
    public class FileAssetDeleterDeleteDatabaseRecordDecorator : IFileAssetDeleter
    {
        private readonly IFileAssetDeleter inner;
        private readonly IEntityStore<FileAsset> fileAssetStore;

        public FileAssetDeleterDeleteDatabaseRecordDecorator(
            IFileAssetDeleter inner,
            IEntityStore<FileAsset> fileAssetStore)
        {
            this.inner = inner;
            this.fileAssetStore = fileAssetStore;
        }

        public async Task<IEnumerable<FileAsset>> RemoveFiles(string[] fileIds)
        {
            var fileLinks = await inner.RemoveFiles(fileIds);
            await fileAssetStore.Delete(fileIds, x => x.FileId);
            return fileLinks;
        }

        public async Task<IEnumerable<FileAsset>> RemoveFile(string fileId)
        {
            await fileAssetStore.Delete(fileId, x => x.FileId);
            return await inner.RemoveFile(fileId);
        }
    }


    public class FileAssetDeleterDereferenceConvoNodesDecorator : IFileAssetDeleter
    {
        private readonly IFileAssetDeleter fileAssetDeleter;
        private readonly IEntityStore<ConversationNode> convoNodeStore;

        public FileAssetDeleterDereferenceConvoNodesDecorator(
            IFileAssetDeleter fileAssetDeleter,
            IEntityStore<ConversationNode> convoNodeStore)
        {
            this.fileAssetDeleter = fileAssetDeleter;
            this.convoNodeStore = convoNodeStore;
        }

        public async Task<IEnumerable<FileAsset>> RemoveFiles(string[] fileIds)
        {
            await DereferenceFileAssetsFromConvoNodes(fileIds);
            return await fileAssetDeleter.RemoveFiles(fileIds);
        }

        public async Task<IEnumerable<FileAsset>> RemoveFile(string fileId)
        {
            await DereferenceFileAssetsFromConvoNodes(new[] { fileId });
            return await fileAssetDeleter.RemoveFile(fileId);
        }

        private async Task DereferenceFileAssetsFromConvoNodes(string[] ids)
        {
            var referencingNodes = await convoNodeStore.GetMany(ids, node => node.NodeId);
            foreach (var node in referencingNodes)
            {
                node.ImageId = null;
            }
        }
    }

    public interface IFileAssetDeleter
    {
        Task<IEnumerable<FileAsset>> RemoveFiles(string[] fileIds);
        Task<IEnumerable<FileAsset>> RemoveFile(string fileId);
    }

    public class FileAssetDeleter : IFileAssetDeleter
    {
        private readonly IEntityStore<FileAsset> fileAssetStore;
        private readonly ICloudDeleter cloudDeleter;

        public FileAssetDeleter(
            IEntityStore<FileAsset> fileAssetStore,
            ICloudDeleter cloudDeleter
        )
        {
            this.fileAssetStore = fileAssetStore;
            this.cloudDeleter = cloudDeleter;
        }

        public async Task<IEnumerable<FileAsset>> RemoveFiles(string[] fileIds)
        {
            var assets = await fileAssetStore.GetMany(fileIds, asset => asset.FileId);
            await fileAssetStore.Delete(assets.ToArray());
            await cloudDeleter.DeleteMany(assets);

            var remainingAssets = await fileAssetStore.GetAll();
            return remainingAssets;
        }

        public async Task<IEnumerable<FileAsset>> RemoveFile(string fileId)
        {
            var asset = await fileAssetStore.Get(fileId, s => s.FileId);
            await fileAssetStore.Delete(asset);
            await cloudDeleter.Delete(asset);

            var remainingAssets = await fileAssetStore.GetAll();
            return remainingAssets;
        }
    }
}