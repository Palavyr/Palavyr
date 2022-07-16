using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Services.FileAssetServices.FileAssetLinkers;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.AttachmentServices
{
    public class AttachmentLinker : IFileAssetLinker<AttachmentLinker>
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly IEntityStore<AttachmentLinkRecord> attachmentLinkRecordStore;
        private readonly IAccountIdTransport accountIdTransport;

        public AttachmentLinker(
            IEntityStore<Intent> intentStore,
            IEntityStore<AttachmentLinkRecord> attachmentLinkRecordStore,
            IAccountIdTransport accountIdTransport)
        {
            this.intentStore = intentStore;
            this.attachmentLinkRecordStore = attachmentLinkRecordStore;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task Link(string fileId, string? intentId)
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

        public async Task Unlink(string fileId, string intentId)
        {
            var intent = await intentStore.GetIntentComplete(intentId);
            var attachment = await attachmentLinkRecordStore.GetMany(fileId, x => x.FileId);

            foreach (var att in attachment)
                intent.AttachmentRecords.Remove(att);
        }
    }
}