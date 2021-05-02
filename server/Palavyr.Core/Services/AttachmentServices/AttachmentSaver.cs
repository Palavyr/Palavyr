using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Common.UIDUtils;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Services.AttachmentServices
{
    public interface IAttachmentSaver
    {
        Task<FileLink> SaveAttachment(string accountId, string areaId, IFormFile attachmentFile);
    }

    public class AttachmentSaver : IAttachmentSaver
    {
        private readonly IS3Saver s3Saver;
        private readonly IConfiguration configuration;
        private readonly ILogger<AttachmentSaver> logger;
        private readonly IS3KeyResolver s3KeyResolver;
        private readonly DashContext dashContext;
        private readonly ILinkCreator linkCreator;
        private readonly ITempPathCreator tempPathCreator;
        private readonly LocalFileDeleter localFileDeleter;

        public AttachmentSaver(
            IS3Saver s3Saver,
            IConfiguration configuration,
            ILogger<AttachmentSaver> logger,
            IS3KeyResolver s3KeyResolver,
            DashContext dashContext,
            ILinkCreator linkCreator,
            ITempPathCreator tempPathCreator,
            LocalFileDeleter localFileDeleter
        )
        {
            this.s3Saver = s3Saver;
            this.configuration = configuration;
            this.logger = logger;
            this.s3KeyResolver = s3KeyResolver;
            this.dashContext = dashContext;
            this.linkCreator = linkCreator;
            this.tempPathCreator = tempPathCreator;
            this.localFileDeleter = localFileDeleter;
        }

        public async Task<FileLink> SaveAttachment(string accountId, string areaId, IFormFile attachmentFile)
        {
            var userDataBucket = configuration.GetUserDataSection();
            var safeFileName = GuidUtils.CreateNewId();
            var riskyFileName = attachmentFile.FileName;
            var s3AttachmentKey = s3KeyResolver.ResolveAttachmentKey(accountId, areaId, safeFileName);

            var fileNameMap = FileNameMap.CreateFileMap(safeFileName, riskyFileName, s3AttachmentKey, accountId, areaId);
            var localTempPath = tempPathCreator.Create(safeFileName + ".pdf");
            
            await using var fileStream = new FileStream(localTempPath, FileMode.Create);
            await attachmentFile.CopyToAsync(fileStream);
            fileStream.Close();

            await s3Saver.SaveObjectToS3(userDataBucket, localTempPath, s3AttachmentKey);
            localFileDeleter.Delete(localTempPath);

            await dashContext.FileNameMaps.AddAsync(fileNameMap); // DB now has s3 key : risky name
            await dashContext.SaveChangesAsync();

            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(s3AttachmentKey, userDataBucket);

            var fileLink = FileLink.CreateLink(riskyFileName, preSignedUrl, safeFileName);
            return fileLink;
        }
    }
}