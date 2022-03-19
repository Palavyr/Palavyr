using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Services.FileAssetServices.FileAssetLinkers;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoAssetSaver : IFileAssetSaver
    {
    }

    public class LogoAssetSaver : ILogoAssetSaver
    {
        private readonly IFileAssetSaver fileAssetSaver;

        public LogoAssetSaver(IFileAssetSaver fileAssetSaver)
        {
            this.fileAssetSaver = fileAssetSaver;
        }

        public async Task<FileAsset> SaveFile(IFormFile fileData)
        {
            var fileAsset = await fileAssetSaver.SaveFile(fileData);
            return fileAsset;
        }
    }

    public class LogoAssetSaverDatabaseUpdaterDecorator : ILogoAssetSaver
    {
        private readonly IFileAssetLinker<LogoLinker> linker;
        private readonly ILogoAssetSaver logoAssetSaver;

        public LogoAssetSaverDatabaseUpdaterDecorator(IFileAssetLinker<LogoLinker> linker, ILogoAssetSaver logoAssetSaver)
        {
            this.linker = linker;
            this.logoAssetSaver = logoAssetSaver;
        }

        public async Task<FileAsset> SaveFile(IFormFile fileData)
        {
            var fileAsset = await logoAssetSaver.SaveFile(fileData);
            await linker.LinkToAccount(fileAsset.FileId);
            return fileAsset;
        }
    }
}