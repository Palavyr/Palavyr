using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.FileAssetServices.FileAssetLinkers
{
    public class LogoLinker : IFileAssetLinker<LogoLinker>
    {
        private readonly IEntityStore<Logo> logoStore;
        private readonly IEntityStore<FileAsset> fileAssetStore;

        public LogoLinker(IEntityStore<Logo> logoStore, IEntityStore<FileAsset> fileAssetStore)
        {
            this.logoStore = logoStore;
            this.fileAssetStore = fileAssetStore;
        }

        public async Task LinkToAccount(string fileId)
        {
            var logoRecord = await logoStore.Get(logoStore.AccountId, s => s.AccountId);
            var fileAsset = await fileAssetStore.Get(fileId, s => fileId);
            logoRecord.AccountLogoFileId = fileAsset.FileId;
        }

        public async Task UnlinkFromAccount(string fileId)
        {
            var logoRecord = await logoStore.Get(logoStore.AccountId, s => s.AccountId);
            logoRecord.AccountLogoFileId = "";
        }

        public Task LinkToIntent(string fileId, string intentId)
        {
            throw new System.NotImplementedException();
        }

        public Task LinkToNode(string fileId, string nodeId)
        {
            throw new System.NotImplementedException();
        }

        public Task UnLinkFromIntent(string fileId, string intentId)
        {
            throw new System.NotImplementedException();
        }

        public Task UnLinkFromNode(string fileId, string nodeId)
        {
            throw new System.NotImplementedException();
        }
    }
}