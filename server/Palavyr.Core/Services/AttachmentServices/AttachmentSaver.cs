using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.LogoServices;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.AttachmentServices
{
    public interface IAttachmentSaver
    {
        Task<FileLink> SaveAttachment(string areaId, IFormFile attachmentFile);
    }

    public class AttachmentSaver : IAttachmentSaver
    {
        private readonly IGuidUtils guidUtils;
        private readonly ICloudFileSaver cloudFileSaver;

        public AttachmentSaver(IGuidUtils guidUtils, ICloudFileSaver cloudFileSaver)
        {
            this.guidUtils = guidUtils;
            this.cloudFileSaver = cloudFileSaver;
        }

        public async Task<FileLink> SaveAttachment(string intentId, IFormFile attachmentFile)
        {
            var safeFileName = guidUtils.CreateNewId();
            var preSignedUrl = await cloudFileSaver.SaveFileAndGetLink(safeFileName, attachmentFile);
            var fileLink = FileLink.CreateUrlLink(attachmentFile.FileName, preSignedUrl, safeFileName);
            return fileLink;
        }
    }

    public interface IFileSaver
    {
        Task<string> SaveFileAndGetUri(string fileName, IFormFile fileData);
    }

    public class FileSaver : IFileSaver
    {
        private readonly ICloudFileSaver cloudFileSaver;

        public FileSaver(ICloudFileSaver cloudFileSaver)
        {
            this.cloudFileSaver = cloudFileSaver;
        }

        public async Task<string> SaveFileAndGetUri(string fileName, IFormFile fileData)
        {
            return await cloudFileSaver.SaveFileAndGetLink(fileName, fileData);
        }
    }

    public class AttachmentSaverDecorator : IAttachmentSaver
    {
        private readonly IAttachmentSaver saver;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly IResolveS3Key<AttachmentKey> resolveS3Key;

        public AttachmentSaverDecorator(
            IAttachmentSaver saver,
            IConfigurationRepository configurationRepository,
            IAccountIdTransport accountIdTransport,
            IResolveS3Key<AttachmentKey> resolveS3Key)
        {
            this.saver = saver;
            this.configurationRepository = configurationRepository;
            this.accountIdTransport = accountIdTransport;
            this.resolveS3Key = resolveS3Key;
        }

        public async Task<FileLink> SaveAttachment(string intentId, IFormFile attachmentFile)
        {
            var fileLink = await saver.SaveAttachment(intentId, attachmentFile);

            var fileName = FileName.ParseFileNameWithStemOverride(attachmentFile.FileName, fileLink.FileName);
            var s3AttachmentKey = resolveS3Key.Resolve(fileName, intentId);
            var fileNameMap = FileNameMap.CreateFileMap(fileLink.FileName, attachmentFile.FileName, s3AttachmentKey, accountIdTransport.AccountId, intentId);
            await configurationRepository.AddFileMap(fileNameMap);

            return fileLink;
        }
    }
}