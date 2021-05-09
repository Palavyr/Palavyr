using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.GlobalConstants;
using Palavyr.Core.Common.UIDUtils;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;

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
        private readonly ITempPathCreator tempPathCreator;
        private readonly DashContext dashContext;
        private readonly ILinkCreator linkCreator;
        private readonly ILocalFileDeleter localFileDeleter;
        private readonly IAccountRepository accountRepository;

        public LogoSaver(
            IS3Saver s3Saver,
            IConfiguration configuration,
            IS3KeyResolver s3KeyResolver,
            ITempPathCreator tempPathCreator,
            DashContext dashContext,
            ILinkCreator linkCreator,
            ILocalFileDeleter localFileDeleter,
           IAccountRepository accountRepository
        )
        {
            this.s3Saver = s3Saver;
            this.configuration = configuration;
            this.s3KeyResolver = s3KeyResolver;
            this.tempPathCreator = tempPathCreator;
            this.dashContext = dashContext;
            this.linkCreator = linkCreator;
            this.localFileDeleter = localFileDeleter;
            this.accountRepository = accountRepository;
        }

        public async Task<string> SaveLogo(string accountId, IFormFile logoFile, CancellationToken cancellationToken)
        {
            var userDataBucket = configuration.GetSection(ConfigSections.UserDataSection).Value;
            var safeFileName = GuidUtils.CreateNewId();
            var riskyFileName = logoFile.FileName;
            var pathExtension = Path.GetExtension(logoFile.FileName);
            if (pathExtension == null) throw new Exception("File type could not be identified");

            var s3AttachmentKey = s3KeyResolver.ResolveLogoKey(accountId, safeFileName, pathExtension);

            var account = await accountRepository.GetAccount(accountId, cancellationToken);
            account.AccountLogoUri = s3AttachmentKey;
            await accountRepository.CommitChangesAsync();

            var fileNameMap = FileNameMap.CreateFileMap(safeFileName, riskyFileName, s3AttachmentKey, accountId, "logo");
            var localTempPath = tempPathCreator.Create(string.Join(".", safeFileName, pathExtension));

            await using var fileStream = new FileStream(localTempPath, FileMode.Create);
            await logoFile.CopyToAsync(fileStream);
            fileStream.Close();

            await s3Saver.SaveObjectToS3(userDataBucket, localTempPath, s3AttachmentKey);
            localFileDeleter.Delete(localTempPath);

            await dashContext.FileNameMaps.AddAsync(fileNameMap); // DB now has s3 key : risky name
            await dashContext.SaveChangesAsync();

            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(s3AttachmentKey, userDataBucket);
            return preSignedUrl;
        }
    }
}