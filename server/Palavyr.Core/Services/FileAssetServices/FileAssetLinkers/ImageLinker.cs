using System.Threading.Tasks;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Services.FileAssetServices.FileAssetLinkers
{
    public class ImageLinker : IFileAssetLinker<ImageLinker>
    {
        private readonly IConfigurationRepository configurationRepository;

        public ImageLinker(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public async Task LinkToNode(string fileId, string nodeId)
        {
            var node = await configurationRepository.GetConversationNodeById(nodeId);
            var fileAsset = await configurationRepository.GetFileAsset(fileId);

            node.ImageId = fileAsset.FileId;
        }

        public async Task UnLinkFromNode(string fileId, string nodeId)
        {
            var node = await configurationRepository.GetConversationNodeById(nodeId);
            var fileAsset = await configurationRepository.GetFileAsset(fileId);

            node.ImageId = fileAsset.FileId;
        }

        public Task LinkToAccount(string fileId)
        {
            throw new System.NotImplementedException();
        }

        public Task UnlinkFromAccount(string fileId)
        {
            throw new System.NotImplementedException();
        }

        public async Task LinkToIntent(string fileId, string intentId)
        {
            throw new System.NotImplementedException();
        }

        public Task UnLinkFromIntent(string fileId, string intentId)
        {
            throw new System.NotImplementedException();
        }

    }
}