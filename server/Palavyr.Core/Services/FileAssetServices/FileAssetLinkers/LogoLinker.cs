using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.FileAssetServices.FileAssetLinkers
{
    public class LogoLinker : IFileAssetLinker<LogoLinker>
    {
        private readonly IEntityStore<Logo> logoStore;

        public LogoLinker(IEntityStore<Logo> logoStore)
        {
            this.logoStore = logoStore;
        }

        public async Task LinkToAccount(string fileId)
        {
            var logoRecord = await logoStore.GetOrCreateLogoRecord();
            logoRecord.AccountLogoFileId = fileId;
        }

        public async Task UnlinkFromAccount(string fileId)
        {
            var logoRecord = await logoStore.GetOrNull(logoStore.AccountId, s => s.AccountId);
            if (logoRecord is null) return;
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