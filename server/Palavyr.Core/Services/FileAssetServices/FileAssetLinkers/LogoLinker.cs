
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.FileAssetServices.FileAssetLinkers
{
    // TODO: each linker type really needs it own implementation. This interface doesn't really capture the truth of these link and unlink methods
    // TODO: Since implementations have unique signature needs, null-ref bugs become a real possibility. If implementations can't adhere to the 
    // TODO: contract while achieving type safety, then the abstraction probably isn't appropriate.
    public class LogoLinker : IFileAssetLinker<LogoLinker>
    {
        private readonly IEntityStore<Logo> logoStore;

        public LogoLinker(IEntityStore<Logo> logoStore)
        {
            this.logoStore = logoStore;
        }

        public async Task Link(string fileId, string _ = null)
        {
            var logoRecord = await logoStore.GetOrCreateLogoRecord();
            logoRecord.AccountLogoFileId = fileId;
        }

        public async Task Unlink(string? fileId, string _ = null)
        {
            var logoRecord = await logoStore.GetOrNull(logoStore.AccountId, s => s.AccountId);
            if (logoRecord is null) return;
            
            // if a fileId is supplied, assume we are looking for a specific use of this file Id with the logo record.
            // .e.g. when deleting the file asset
            if (!string.IsNullOrEmpty(fileId))
            {
                if (logoRecord.AccountLogoFileId == fileId)
                {
                    logoRecord.AccountLogoFileId = "";
                }
            }
            // Else, we are requesting a generic dereference of the account logo id (we delete the logo, but not the fileAsset itself)
            else
            {
                logoRecord.AccountLogoFileId = "";
            }
        }
    }
}