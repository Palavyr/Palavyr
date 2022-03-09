using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;
using Palavyr.Core.Services.FileAssetServices.FileAssetLinkers;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.AttachmentServices
{
    public class AttachmentLinker : IFileAssetLinker<AttachmentLinker>
    {
        private readonly IConfigurationEntityStore<Area> intentStore;
        private readonly IConfigurationEntityStore<AttachmentLinkRecord> attachmentLinkRecordStore;
        private readonly IConfigurationEntityStore<ConversationNode> convoNodeStore;
        private readonly IConfigurationEntityStore<FileAsset> fileAssetStore;
        private readonly IAccountIdTransport accountIdTransport;

        public AttachmentLinker(
            IConfigurationEntityStore<Area> intentStore,
            IConfigurationEntityStore<AttachmentLinkRecord> attachmentLinkRecordStore,
            IConfigurationEntityStore<ConversationNode> convoNodeStore,
            IConfigurationEntityStore<FileAsset> fileAssetStore,
            IAccountIdTransport accountIdTransport)
        {
            this.intentStore = intentStore;
            this.attachmentLinkRecordStore = attachmentLinkRecordStore;
            this.convoNodeStore = convoNodeStore;
            this.fileAssetStore = fileAssetStore;
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

            await attachmentLinkRecordStore.Create(attachment);
            var intent = await intentStore.GetIntentComplete(intentId);
            intent.AttachmentRecords.Add(attachment);
        }

        public async Task LinkToNode(string fileId, string nodeId)
        {
            var node = await convoNodeStore.Get(nodeId, s => s.NodeId);
            var fileAsset = await fileAssetStore.Get(fileId, s => s.FileId);
            node.ImageId = fileAsset.FileId;
        }

        public Task LinkToAccount(string fileId)
        {
            throw new System.NotImplementedException();
        }

        public async Task UnLinkFromIntent(string fileId, string intentId)
        {
            var intent = await intentStore.GetIntentComplete(intentId);
            var attachment = await attachmentLinkRecordStore.Get(fileId, x => x.FileId);
            intent.AttachmentRecords.Remove(attachment);
        }

        public async Task UnLinkFromNode(string fileId, string nodeId)
        {
            var node = await convoNodeStore.Get(nodeId, t => t.NodeId);
            var fileAsset = await fileAssetStore.Get(fileId, p => p.FileId);
            node.ImageId = fileAsset.FileId;
        }

        public Task UnlinkFromAccount(string fileId)
        {
            throw new System.NotImplementedException();
        }
    }
}