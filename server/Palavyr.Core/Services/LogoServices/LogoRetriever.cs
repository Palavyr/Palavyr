
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoRetriever
    {
        Task<FileAsset?> GetLogo();
        Task<FileAsset?> GetLogoThumbnail();
    }

    public class LogoRetriever : ILogoRetriever
    {
        private readonly IEntityStore<Logo> logoStore;
        private readonly IEntityStore<FileAsset> fileAssetStore;

        public LogoRetriever(
            IEntityStore<Logo> logoStore,
            IEntityStore<FileAsset> fileAssetStore )
        {
            this.logoStore = logoStore;
            this.fileAssetStore = fileAssetStore;
        }

        public async Task<FileAsset?> GetLogo()
        {
            var logo = await logoStore.GetOrNull(logoStore.AccountId, x => x.AccountId);
            if (logo is null || string.IsNullOrEmpty(logo.AccountLogoFileId)) return null;
            var fileAsset = await fileAssetStore.Get(logo.AccountLogoFileId, s => s.FileId);
            return fileAsset;
        }

        public Task<FileAsset?> GetLogoThumbnail()
        {
            throw new System.NotImplementedException();
        }
    }
}