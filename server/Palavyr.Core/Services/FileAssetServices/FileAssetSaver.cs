using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.FileAssetServices
{
    public class FileAssetSaverDatabaseDecorator : IFileAssetSaver
    {
        private readonly IFileAssetSaver inner;
        private readonly IEntityStore<FileAsset> fileAssetStore;

        public FileAssetSaverDatabaseDecorator(
            IFileAssetSaver inner,
            IEntityStore<FileAsset> fileAssetStore)
        {
            this.inner = inner;
            this.fileAssetStore = fileAssetStore;
        }

        public async Task<FileAsset> SaveFile(IFormFile fileData)
        {
            var fileAsset = await inner.SaveFile(fileData);
            await fileAssetStore.Create(fileAsset);
            await fileAssetStore.Get(fileAsset.FileId, s => s.FileId);
            return fileAsset;
        }
    }


    public interface IFileAssetSaver
    {
        Task<FileAsset> SaveFile(IFormFile fileData);
    }

    public class FileAssetSaver : IFileAssetSaver
    {
        private readonly ICloudFileSaver cloudFileSaver;
        private readonly IGuidUtils guidUtils;

        public FileAssetSaver(ICloudFileSaver cloudFileSaver, IGuidUtils guidUtils)
        {
            this.cloudFileSaver = cloudFileSaver;
            this.guidUtils = guidUtils;
        }

        public async Task<FileAsset> SaveFile(IFormFile fileData)
        {
            var fileName = FileName.ParseRiskyFileName(fileData.FileName, guidUtils.CreateNewId());
            var fileAsset = await cloudFileSaver.SaveFile(fileName, fileData);
            return fileAsset;
        }
    }
}