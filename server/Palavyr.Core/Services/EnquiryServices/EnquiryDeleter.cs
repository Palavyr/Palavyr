using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.FileAssetServices;

namespace Palavyr.Core.Services.EnquiryServices
{
    public interface IEnquiryDeleter
    {
        Task DeleteEnquiry(string fileId, CancellationToken cancellationToken);
        Task DeleteEnquiries(string[] fileReferences, CancellationToken cancellationToken);
    }

    public class EnquiryDeleter : IEnquiryDeleter
    {
        private readonly IS3FileDeleter is3FileDeleter;
        private readonly IFileAssetDeleter fileAssetDeleter;
        private readonly IConfiguration configuration;
        private readonly ConvoContext convoContext;

        public EnquiryDeleter(
            IFileAssetDeleter fileAssetDeleter,
            IConfiguration configuration,
            ConvoContext convoContext
        )
        {
            this.fileAssetDeleter = fileAssetDeleter;
            this.configuration = configuration;
            this.convoContext = convoContext;
        }

        public async Task DeleteEnquiry(string conversationId, CancellationToken cancellationToken)
        {
            await DeleteFromS3(conversationId);
            TrackDeleteFromDb(conversationId);
            await convoContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteEnquiries(string[] fileReferences, CancellationToken cancellationToken)
        {
            foreach (var fileReference in fileReferences)
            {
                await DeleteEnquiry(fileReference, cancellationToken);
            }

            await convoContext.SaveChangesAsync(cancellationToken);
        }

        private async Task DeleteFromS3(string fileId)
        {
            await fileAssetDeleter.RemoveFile(fileId);
        }

        public void TrackDeleteFromDb(string conversationId)
        {
            var rowsToDelete = convoContext.ConversationRecords.Where(x => x.ConversationId == conversationId);
            convoContext.ConversationRecords.RemoveRange(rowsToDelete);
        }
    }
}