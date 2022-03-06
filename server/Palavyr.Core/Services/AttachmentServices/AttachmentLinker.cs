using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.FileAssetServices.FileAssetLinkers;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.AttachmentServices
{
    public class AttachmentLinker : IFileAssetLinker<AttachmentLinker>
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IAccountIdTransport accountIdTransport;

        public AttachmentLinker(IConfigurationRepository configurationRepository, IAccountIdTransport accountIdTransport)
        {
            this.configurationRepository = configurationRepository;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task LinkToIntent(string fileId, string intentId)
        {
            var attachment = new AttachmentLinkRecord
            {
                AccountId = accountIdTransport.AccountId,
                FileId = fileId,
                IntentId = intentId
            };

            await configurationRepository.CreateAttachmentLinkRecord(attachment);

            var intent = await configurationRepository.GetAreaByIdWithAttachments(intentId);

            intent.AttachmentRecords.Add(attachment);
        }

        public async Task LinkToNode(string fileId, string nodeId)
        {
            var node = await configurationRepository.GetConversationNodeById(nodeId);
            var fileAsset = await configurationRepository.GetFileAsset(fileId);

            node.ImageId = fileAsset.FileId;
        }

        public Task LinkToAccount(string fileId)
        {
            throw new System.NotImplementedException();
        }

        public async Task UnLinkFromIntent(string fileId, string intentId)
        {
            var intent = await configurationRepository.GetAreaByIdWithAttachments(intentId);
            var attachment = await configurationRepository.GetAttachmentRecord(fileId);

            intent.AttachmentRecords.Remove(attachment);
        }

        public async Task UnLinkFromNode(string fileId, string nodeId)
        {
            var node = await configurationRepository.GetConversationNodeById(nodeId);
            var fileAsset = await configurationRepository.GetFileAsset(fileId);

            node.ImageId = fileAsset.FileId;
        }

        public Task UnlinkFromAccount(string fileId)
        {
            throw new System.NotImplementedException();
        }
    }
}