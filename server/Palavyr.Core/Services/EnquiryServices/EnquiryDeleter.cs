using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.EnquiryServices
{
    public interface IEnquiryDeleter
    {
        Task DeleteEnquiry(string fileId, CancellationToken cancellationToken);
        Task DeleteEnquiries(string[] fileReferences, CancellationToken cancellationToken);
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

        public async Task DeleteEnquiry(string conversationId, CancellationToken cancellationToken)
        {
            await DeleteFromS3(conversationId);
            await TrackDeleteFromDb(conversationId);
        }

        public async Task DeleteEnquiries(string[] fileReferences, CancellationToken cancellationToken)
        {
            foreach (var fileReference in fileReferences)
            {
                await DeleteEnquiry(fileReference, cancellationToken);
            }
        }

        private async Task DeleteFromS3(string fileId)
        {
            await fileAssetDeleter.RemoveFile(fileId);
        }

        public async Task TrackDeleteFromDb(string conversationId)
        {
            await convoRecordStore.Delete(conversationId, s => s.ConversationId);
        }
    }
}