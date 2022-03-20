using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.FileAssetServices.FileAssetLinkers;
using Palavyr.Core.Sessions;
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


    public class FileAssetDeleterDereferenceDecorator : IFileAssetDeleter
    {
        private readonly IFileAssetDeleter fileAssetDeleter;
        private readonly IEntityStore<AttachmentLinkRecord> attachmentLinkRecordStore;
        private readonly IFileAssetLinker<LogoLinker> logoLinker;
        private readonly IFileAssetLinker<AttachmentLinker> attachmentLinker;
        private readonly IFileAssetLinker<NodeLinker> nodeLinker;
        private readonly IAccountIdTransport accountIdTransport;
        private string AccountId => accountIdTransport.AccountId;

        public FileAssetDeleterDereferenceDecorator(
            IFileAssetDeleter fileAssetDeleter,
            IEntityStore<AttachmentLinkRecord> attachmentLinkRecordStore,
            IFileAssetLinker<LogoLinker> logoLinker,
            IFileAssetLinker<AttachmentLinker> attachmentLinker,
            IFileAssetLinker<NodeLinker> nodeLinker,
            IAccountIdTransport accountIdTransport)
        {
            this.fileAssetDeleter = fileAssetDeleter;
            this.attachmentLinkRecordStore = attachmentLinkRecordStore;
            this.logoLinker = logoLinker;
            this.attachmentLinker = attachmentLinker;
            this.nodeLinker = nodeLinker;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<IEnumerable<FileAsset>> RemoveFiles(string[] fileIds)
        {
            await DereferenceAllLocations(fileIds);
            return await fileAssetDeleter.RemoveFiles(fileIds);
        }

        public async Task<IEnumerable<FileAsset>> RemoveFile(string fileId)
        {
            await DereferenceAllLocations(new[] { fileId });
            return await fileAssetDeleter.RemoveFile(fileId);
        }

        private async Task DereferenceAllLocations(string[] fileIds)
        {
            await DereferenceFileAssetsFromConvoNodes(fileIds);
            await DereferenceIntents(fileIds);
            await DereferenceLogo(fileIds);
        }

        private async Task DereferenceLogo(string[] fileIds)
        {
            foreach (var fileId in fileIds)
            {
                // try to unlink if its used in the logo. It may not be.
                await logoLinker.Unlink(fileId, AccountId);
            }
        }

        private async Task DereferenceFileAssetsFromConvoNodes(string[] fileIds)
        {
            foreach (var fileId in fileIds)
            {
                await nodeLinker.Unlink(fileId, default);
            }
        }

        private async Task DereferenceIntents(string[] fileIds)
        {
            foreach (var fileId in fileIds)
            {
                var attachmentRecords = await attachmentLinkRecordStore.GetMany(fileId, s => s.FileId);
                var intentIds = attachmentRecords.Select(x => x.IntentId);
                foreach (var intentId in intentIds)
                {
                    await attachmentLinker.Unlink(fileId, intentId);
                }
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
            return remainingAssets.Where(x => !fileIds.Contains(x.FileId));
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