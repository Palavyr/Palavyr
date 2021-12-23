using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.API.Controllers.Enquiries
{
    public interface IEnquiryDeleter
    {
        Task DeleteEnquiry(string fileId, CancellationToken cancellationToken);
        Task DeleteEnquiries(string[] fileReferences, CancellationToken cancellationToken);   
    }

    public class EnquiryDeleter : IEnquiryDeleter
    {
        private readonly IS3Deleter s3Deleter;
        private readonly IS3KeyResolver s3KeyResolver;
        private readonly IConfiguration configuration;
        private readonly ConvoContext convoContext;

        public EnquiryDeleter(
            IS3Deleter s3Deleter,
            IS3KeyResolver s3KeyResolver,
            IConfiguration configuration,
            ConvoContext convoContext
        )
        {
            this.s3Deleter = s3Deleter;
            this.s3KeyResolver = s3KeyResolver;
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

        private async Task DeleteFromS3(string fileReference)
        {
            // Delete from S3
            var s3Key = s3KeyResolver.ResolveResponsePdfKey(fileReference);
            var userDataBucket = configuration.GetUserDataBucket();
            var success = await s3Deleter.DeleteObjectFromS3Async(userDataBucket, s3Key);
            if (!success)
            {
                throw new Exception("Failed to delete s3 file.");
            }
        }

        public void TrackDeleteFromDb(string conversationId)
        {
            var rowsToDelete = convoContext.ConversationRecords.Where(x => x.ConversationId == conversationId);
            convoContext.ConversationRecords.RemoveRange(rowsToDelete);
        }
    }
}