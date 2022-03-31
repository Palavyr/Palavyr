using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Mappers;
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
        private readonly IEntityStore<FileAsset> fileAssetStore;

        public FileAssetSaver(ICloudFileSaver cloudFileSaver, IGuidUtils guidUtils, IEntityStore<FileAsset> fileAssetStore)
        {
            this.cloudFileSaver = cloudFileSaver;
            this.guidUtils = guidUtils;
            this.fileAssetStore = fileAssetStore;
        }

        public async Task<FileAsset> SaveFile(IFormFile fileData)
        {
            var fileName = await GenerateFileName(fileData.FileName);
            var fileAsset = await cloudFileSaver.SaveFile(fileName, fileData);
            return fileAsset;
        }

        private async Task<FileName> GenerateFileName(string providedFilename)
        {
            var fileName = FileName.ParseRiskyFileName(providedFilename, guidUtils.CreateNewId());
            var currentFiles = await fileAssetStore.GetAll();
            var currentFileNames = currentFiles.Select(x => x.RiskyNameWithExtension.ToLowerInvariant()).ToArray();
            if (currentFileNames.Contains(providedFilename.ToLowerInvariant()))
            {
                throw new DomainException("That file already exists!");
            }

            return fileName;
        }
    }
}