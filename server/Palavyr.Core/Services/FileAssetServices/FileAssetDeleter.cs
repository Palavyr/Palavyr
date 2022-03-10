using System.Threading.Tasks;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;

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

        public async Task<FileLink[]> RemoveFiles(string[] fileIds)
        {
            var fileLinks = await inner.RemoveFiles(fileIds);
            await fileAssetStore.Delete(fileIds, x => x.FileId);
            return fileLinks;
        }

        public async Task<FileLink[]> RemoveFile(string fileId)
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

        public async Task<FileLink[]> RemoveFiles(string[] fileIds)
        {
            await DereferenceFileAssetsFromConvoNodes(fileIds);
            return await fileAssetDeleter.RemoveFiles(fileIds);
        }

        public async Task<FileLink[]> RemoveFile(string fileId)
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
        Task<FileLink[]> RemoveFiles(string[] fileIds);
        Task<FileLink[]> RemoveFile(string fileId);
    }

    public class FileAssetDeleter : IFileAssetDeleter
    {
        private readonly IEntityStore<FileAsset> fileAssetStore;
        private readonly ICloudDeleter cloudDeleter;
        private readonly ILinkCreator linkCreator;

        public FileAssetDeleter(
            IEntityStore<FileAsset> fileAssetStore,
            ICloudDeleter cloudDeleter,
            ILinkCreator linkCreator
        )
        {
            this.fileAssetStore = fileAssetStore;
            this.cloudDeleter = cloudDeleter;
            this.linkCreator = linkCreator;
        }

        public async Task<FileLink[]> RemoveFiles(string[] fileIds)
        {
            var images = await fileAssetStore.GetMany(fileIds, asset => asset.FileId);
            await cloudDeleter.DeleteMany(images);

            var remainingAssets = await fileAssetStore.GetAll();
            return await remainingAssets.ToFileLinks(linkCreator);
        }

        public async Task<FileLink[]> RemoveFile(string fileId)
        {
            await fileAssetStore.Delete(fileId, x => x.FileId);
            var remainingAssets = await fileAssetStore.GetAll();
            return await remainingAssets.ToFileLinks(linkCreator);
        }
    }
}