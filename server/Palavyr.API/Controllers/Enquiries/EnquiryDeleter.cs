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
        Task DeleteEnquiry(string accountId, string fileId, CancellationToken cancellationToken);
        Task DeleteEnquiries(string accountId, string[] fileReferences, CancellationToken cancellationToken);   
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


        public async Task DeleteEnquiry(string accountId, string fileReference, CancellationToken cancellationToken)
        {
            await DeleteFromS3(accountId, fileReference);
            TrackDeleteFromDb(fileReference);
            await convoContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteEnquiries(string accountId, string[] fileReferences, CancellationToken cancellationToken)
        {
            foreach (var fileReference in fileReferences)
            {
                await DeleteEnquiry(accountId, fileReference, cancellationToken);
                TrackDeleteFromDb(fileReference);
            }

            await convoContext.SaveChangesAsync(cancellationToken);
        }

        private async Task DeleteFromS3(string accountId, string fileReference)
        {
            // Delete from S3
            var s3Key = s3KeyResolver.ResolveResponsePdfKey(accountId, fileReference);
            var userDataBucket = configuration.GetUserDataSection();
            var success = await s3Deleter.DeleteObjectFromS3Async(userDataBucket, s3Key);
            if (!success)
            {
                throw new Exception("Failed to delete s3 file.");
            }
        }

        public void TrackDeleteFromDb(string fileReference)
        {
            var rowsToDelete = convoContext.CompletedConversations.Where(x => x.ConversationId == fileReference);
            convoContext.CompletedConversations.RemoveRange(rowsToDelete);
        }
    }
}