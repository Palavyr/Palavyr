using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.CloudKeyResolvers;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Test.Common.TestFileAssetServices
{
    public class IntegrationTestFileSaver : IFileAssetSaver
    {
        private readonly IAccountIdTransport accountIdTransport;
        private readonly ICancellationTokenTransport cancellationTokenTransport;
        private readonly IGuidUtils guidUtils;
        private readonly IFileAssetKeyResolver fileAssetKeyResolver;
        private string IsolatedStorageDirectory => IsolatedStorageFile.GetMachineStoreForApplication().GetStorageDirectory();
        private CancellationToken CancellationToken => cancellationTokenTransport.CancellationToken;
        private string AccountId => accountIdTransport.AccountId;

        public IntegrationTestFileSaver(
            IAccountIdTransport accountIdTransport,
            ICancellationTokenTransport cancellationTokenTransport,
            IGuidUtils guidUtils,
            IFileAssetKeyResolver fileAssetKeyResolver)
        {
            this.accountIdTransport = accountIdTransport;
            this.cancellationTokenTransport = cancellationTokenTransport;
            this.guidUtils = guidUtils;
            this.fileAssetKeyResolver = fileAssetKeyResolver;
        }

        public async Task<FileAsset> SaveFile(IFormFile fileData)
        {
            var fileName = FileName.ParseRiskyFileName(fileData.FileName, guidUtils.CreateNewId());
            var fileKey = fileAssetKeyResolver.Resolve(fileName);

            var localLocationKey = Path.Join(IsolatedStorageDirectory, fileKey);
            using (Stream fileStream = new FileStream(localLocationKey, FileMode.Create))
            {
                await fileData.CopyToAsync(fileStream, CancellationToken);

                return new FileAsset
                {
                    Extension = fileName.Extension,
                    AccountId = AccountId,
                    FileId = fileName.FileId,
                    LocationKey = localLocationKey,
                    RiskyNameStem = fileName.FileStem
                };
            }
        }
    }

    public class IntegrationTestFileDelete : IFileAssetDeleter
    {
        private readonly IEntityStore<FileAsset> fileAssetStore;

        public IntegrationTestFileDelete(IEntityStore<FileAsset> fileAssetStore)
        {
            this.fileAssetStore = fileAssetStore;
        }

        public async Task<IEnumerable<FileAsset>> RemoveFiles(string[] fileIds)
        {
            var assets = await fileAssetStore.GetMany(fileIds, asset => asset.FileId);
            await fileAssetStore.Delete(assets.ToArray());
            foreach (var asset in assets)
            {
                if (File.Exists(asset.LocationKey))
                {
                    File.Delete(asset.LocationKey);
                }
            }

            var remainingAssets = await fileAssetStore.GetAll();
            return remainingAssets;
        }

        public async Task<IEnumerable<FileAsset>> RemoveFile(string fileId)
        {
            var asset = await fileAssetStore.Get(fileId, s => s.FileId);
            await fileAssetStore.Delete(asset);
            if (File.Exists(asset.LocationKey))
            {
                File.Delete(asset.LocationKey);
            }

            var remainingAssets = await fileAssetStore.GetAll();
            return remainingAssets;
        }
    }

    public class IntegrationTestFileLinkCreator : ILinkCreator
    {
        private readonly IEntityStore<FileAsset> fileAssetStore;

        public IntegrationTestFileLinkCreator(
            IEntityStore<FileAsset> fileAssetStore
        )
        {
            this.fileAssetStore = fileAssetStore;
        }

        public async Task<string> CreateLink(string fileAssetId)
        {
            var asset = await fileAssetStore.Get(fileAssetId, s => s.FileId);
            return asset.LocationKey ?? "";
        }

        public string CreateLink(FileAsset fileAsset)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string[]> CreateManyLinks(string[] fileAssetIds)
        {
            var assets = await fileAssetStore.GetMany(fileAssetIds, s => s.FileId);
            return assets.Select(x => x.LocationKey).ToArray();
        }

        public async Task<IEnumerable<string>> CreateManyLinks(IEnumerable<string> fileAssetIds)
        {
            return await CreateManyLinks(fileAssetIds);
        }
    }
}