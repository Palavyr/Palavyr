using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.GlobalConstants;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.API.Controllers.Attachments
{
    public interface IAttachmentDeleter
    {
        Task DeleteAttachment(string fileId, CancellationToken cancellationToken);
        Task DeleteAllAreaAttachments(string areaId, CancellationToken cancellationToken);
    }

    public class AttachmentDeleter : IAttachmentDeleter
    {
        private readonly IConfiguration configuration;
        private readonly DashContext dashContext;

        private readonly IS3Deleter s3Deleter;

        public AttachmentDeleter(
            IConfiguration configuration,
            DashContext dashContext,
            IS3Deleter s3Deleter)
        {
            this.configuration = configuration;
            this.dashContext = dashContext;
            this.s3Deleter = s3Deleter;
        }

        public async Task DeleteAttachment(string fileId, CancellationToken cancellationToken)
        {
            var userDataBucket = configuration.GetSection(ConfigSections.UserDataSection).Value;
            var meta = await dashContext.FileNameMaps.SingleAsync(x => x.SafeName == fileId, cancellationToken);
            var success = await s3Deleter.DeleteObjectFromS3Async(userDataBucket, meta.S3Key);
            if (!success)
            {
                throw new AmazonS3Exception($"Could not Delete {meta.S3Key} from {userDataBucket}");
            }

            dashContext.FileNameMaps.Remove(meta);
            await dashContext.SaveChangesAsync();
        }

        public async Task DeleteAllAreaAttachments(string areaId, CancellationToken cancellationToken)
        {
            var userDataBucket = configuration.GetSection(ConfigSections.UserDataSection).Value;
            var metas = dashContext.FileNameMaps
                .Where(x => x.AreaIdentifier == areaId);
            var keys = await metas
                .Select(x => x.S3Key)
                .ToArrayAsync(cancellationToken);
            var success = await s3Deleter.DeleteObjectsFromS3Async(userDataBucket, keys);
            if (!success)
            {
                throw new AmazonS3Exception($"Could not Delete {string.Join(", ", keys)} from {userDataBucket}");
            }

            dashContext.FileNameMaps.RemoveRange(metas);
            await dashContext.SaveChangesAsync();
        }
    }
}