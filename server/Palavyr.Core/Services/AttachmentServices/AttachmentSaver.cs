﻿using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Common.FileSystemTools;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.TemporaryPaths;

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
        private readonly ITemporaryPath temporaryPath;
        private readonly ILocalIo localIo;

        public AttachmentSaver(
            IS3Saver s3Saver,
            IConfiguration configuration,
            ILogger<AttachmentSaver> logger,
            IS3KeyResolver s3KeyResolver,
            DashContext dashContext,
            ILinkCreator linkCreator,
            ITemporaryPath temporaryPath,
            ILocalIo localIo
        )
        {
            this.s3Saver = s3Saver;
            this.configuration = configuration;
            this.logger = logger;
            this.s3KeyResolver = s3KeyResolver;
            this.dashContext = dashContext;
            this.linkCreator = linkCreator;
            this.temporaryPath = temporaryPath;
            this.localIo = localIo;
        }

        public async Task<FileLink> SaveAttachment(string accountId, string areaId, IFormFile attachmentFile)
        {
            var userDataBucket = configuration.GetUserDataSection();
            var safeFileName = GuidUtils.CreateNewId();
            var riskyFileName = attachmentFile.FileName;
            var s3AttachmentKey = s3KeyResolver.ResolveAttachmentKey(accountId, areaId, safeFileName);

            var fileNameMap = FileNameMap.CreateFileMap(safeFileName, riskyFileName, s3AttachmentKey, accountId, areaId);
            var localTempSafeFile = temporaryPath.CreateLocalTempSafeFile();

            await localIo.SaveFile(localTempSafeFile.S3Key, attachmentFile);
            
            await s3Saver.SaveObjectToS3(userDataBucket, localTempSafeFile.S3Key, s3AttachmentKey);
            temporaryPath.DeleteLocalTempFile(localTempSafeFile.FileNameWithExtension);
      
            await dashContext.FileNameMaps.AddAsync(fileNameMap); // DB now has s3 key : risky name
            await dashContext.SaveChangesAsync();

            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(s3AttachmentKey, userDataBucket);

            var fileLink = FileLink.CreateLink(riskyFileName, preSignedUrl, safeFileName);
            return fileLink;
        }
    }
}