using System.Threading.Tasks;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Services.FileAssetServices.FileAssetLinkers
{
    public class LogoLinker : IFileAssetLinker<LogoLinker>
    {
        private readonly IConfigurationRepository configurationRepository;

        public LogoLinker(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task LinkToAccount(string fileId)
        {
            var logoRecord = await configurationRepository.GetAccountLogo();
            var fileAsset = await configurationRepository.GetFileAsset(fileId);
            logoRecord.AccountLogoFileId = fileAsset.FileId;
        }

        public async Task UnlinkFromAccount(string fileId)
        {
            var logoRecord = await configurationRepository.GetAccountLogo();
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