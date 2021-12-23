using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.FileSystemTools;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.TemporaryPaths;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoSaver
    {
        Task<string> SaveLogo(IFormFile logoFile);
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

        public async Task<string> SaveLogo(IFormFile logoFile)
        {
            var userDataBucket = configuration.GetSection(ApplicationConstants.ConfigSections.UserDataSection).Value;
            var localSafePath = temporaryPath.CreateLocalTempSafeFile();

            var pathExtension = Path.GetExtension(logoFile.FileName);
            if (pathExtension == null) throw new Exception("File type could not be identified");

            var logoKey = s3KeyResolver.ResolveLogoKey(localSafePath.FileStem, pathExtension);

            var account = await accountRepository.GetAccount();
            account.AccountLogoUri = logoKey;
            await accountRepository.CommitChangesAsync();

            await localIo.SaveFile(localSafePath.S3Key, logoFile);

            await s3Saver.StreamObjectToS3(userDataBucket, logoFile, logoKey);
            temporaryPath.DeleteLocalTempFile(localSafePath.FileNameWithExtension);

            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(logoKey, userDataBucket);
            return preSignedUrl;
        }
    }
}