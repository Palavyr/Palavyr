using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.FileSystemTools;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.TemporaryPaths;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoSaver
    {
        Task<string> SaveLogo(string accountId, IFormFile logoFile, CancellationToken cancellationToken);
    }

    public class LogoSaver : ILogoSaver
    {
        private readonly IS3Saver s3Saver;
        private readonly IConfiguration configuration;
        private readonly IS3KeyResolver s3KeyResolver;
        private readonly ITemporaryPath temporaryPath;
        private readonly ILinkCreator linkCreator;
        private readonly IAccountRepository accountRepository;
        private readonly ILocalIo localIo;

        public LogoSaver(
            IS3Saver s3Saver,
            IConfiguration configuration,
            IS3KeyResolver s3KeyResolver,
            ITemporaryPath temporaryPath,
            ILinkCreator linkCreator,
            IAccountRepository accountRepository,
            ILocalIo localIo
        )
        {
            this.s3Saver = s3Saver;
            this.configuration = configuration;
            this.s3KeyResolver = s3KeyResolver;
            this.temporaryPath = temporaryPath;
            this.linkCreator = linkCreator;
            this.accountRepository = accountRepository;
            this.localIo = localIo;
        }

        public async Task<string> SaveLogo(string accountId, IFormFile logoFile, CancellationToken cancellationToken)
        {
            var userDataBucket = configuration.GetSection(ApplicationConstants.ConfigSections.UserDataSection).Value;
            var localSafePath = temporaryPath.CreateLocalTempSafeFile();

            var pathExtension = Path.GetExtension(logoFile.FileName);
            if (pathExtension == null) throw new Exception("File type could not be identified");

            var s3AttachmentKey = s3KeyResolver.ResolveLogoKey(accountId, localSafePath.FileStem, pathExtension);

            var account = await accountRepository.GetAccount(accountId, cancellationToken);
            account.AccountLogoUri = s3AttachmentKey;
            await accountRepository.CommitChangesAsync();

            await localIo.SaveFile(localSafePath.FullPath, logoFile);

            await s3Saver.SaveObjectToS3(userDataBucket, localSafePath.FullPath, s3AttachmentKey);
            temporaryPath.DeleteLocalTempFile(localSafePath.FileNameWithExtension);

            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(s3AttachmentKey, userDataBucket);
            return preSignedUrl;
        }
    }
}