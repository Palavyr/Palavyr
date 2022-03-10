using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.FileAssetServices.FileAssetLinkers;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoDeleter
    {
        Task DeleteLogo();
    }

    public class LogoDeleter : ILogoDeleter
    {
        private readonly IEntityStore<Logo> logoStore;
        private readonly IFileAssetLinker<LogoLinker> logoLinker;


        public LogoDeleter(
            IEntityStore<Logo> logoStore,
            IFileAssetLinker<LogoLinker> logoLinker
        )
        {
            this.logoStore = logoStore;
            this.logoLinker = logoLinker;
        }

        public async Task DeleteLogo()
        {
            var logo = await logoStore.Get(logoStore.AccountId, s => s.AccountId);
            await logoLinker.UnlinkFromAccount(logo.AccountLogoFileId);
        }
    }
}