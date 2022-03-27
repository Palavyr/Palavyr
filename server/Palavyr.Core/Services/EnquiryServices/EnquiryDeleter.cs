using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.EnquiryServices
{
    public interface IEnquiryDeleter
    {
        Task DeleteEnquiries(string[] fileReferences);
    }

    public class EnquiryDeleter : IEnquiryDeleter
    {
        private readonly IFileAssetDeleter fileAssetDeleter;
        private readonly IEntityStore<ConversationRecord> convoRecordStore;

        public EnquiryDeleter(
            IFileAssetDeleter fileAssetDeleter,
            IEntityStore<ConversationRecord> convoRecordStore
        )
        {
            this.fileAssetDeleter = fileAssetDeleter;
            this.convoRecordStore = convoRecordStore;
        }

        public async Task DeleteEnquiries(string[] conversationIds)
        {
            var fileIds = new List<string>();
            foreach (var conversationId in conversationIds)
            {
                var record = await convoRecordStore.Get(conversationId, s => s.ConversationId);

                fileIds.Add(record.ResponsePdfId);
                await convoRecordStore.Delete(conversationId, s => s.ConversationId);
            }

            await fileAssetDeleter.RemoveFiles(fileIds.ToArray());
        }
    }
}