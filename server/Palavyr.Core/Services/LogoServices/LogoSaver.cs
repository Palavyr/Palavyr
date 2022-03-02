using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Exceptions;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Services.LogoServices
{
    public interface ILogoSaver
    {
        Task<string> SaveLogo(IFormFile logoFile);
    }

    public class LogoSaver : ILogoSaver
    {
        private readonly ICloudFileSaver cloudFileSaver;

        public LogoSaver(ICloudFileSaver cloudFileSaver)
        {
            this.cloudFileSaver = cloudFileSaver;
        }

        public async Task<string> SaveLogo(IFormFile logoFile)
        {
            var pathExtension = Path.GetExtension(logoFile.FileName);
            if (pathExtension == null) throw new DomainException("File type could not be identified. Please provide a valid image file (.png, .jpg, or .svg");

            return await cloudFileSaver.SaveFileAndGetLink(logoFile.FileName, logoFile);
        }
    }

    public interface ICloudFileSaver
    {
        Task<FileLocation> SaveFile(string fileNameWithSuffix, IFormFile fileData);

        Task<string> SaveFileAndGetLink(string fileNameWithSuffix, IFormFile fileData);
        
        FileLocation GetFileLocation(string fileNameWithSuffix);
    }


    public class AwsCloudFileSaver : ICloudFileSaver
    {
        private readonly IS3Saver s3Saver;
        private readonly IResolveS3Key<LogoKey> keyResolver;
        private readonly ILinkCreator linkCreator;

        private readonly IConfiguration configuration;
        private readonly IGuidUtils guidUtils;

        public AwsCloudFileSaver(
            IS3Saver s3Saver,
            IResolveS3Key<LogoKey> keyResolver,
            ILinkCreator linkCreator,
            IConfiguration configuration,
            IGuidUtils guidUtils
        )
        {
            this.s3Saver = s3Saver;
            this.keyResolver = keyResolver;
            this.linkCreator = linkCreator;
            this.configuration = configuration;
            this.guidUtils = guidUtils;
        }

        public async Task<FileLocation> SaveFile(string fileNameWithExtension, IFormFile fileData)
        {
            var safeFileName = guidUtils.CreateNewId();
            var logoKey = keyResolver.Resolve(FileName.ParseFileNameWithStemOverride(fileNameWithExtension, safeFileName));

            var userDataBucket = configuration.GetSection(ApplicationConstants.ConfigSections.UserDataSection).Value;
            await s3Saver.StreamObjectToS3(userDataBucket, fileData, logoKey);

            return new FileLocation
            {
                CloudType = CloudType.Aws,
                FileUri = logoKey,
                ValueForDatabase = logoKey
            };
        }

        public async Task<string> SaveFileAndGetLink(string fileNameWithSuffix, IFormFile fileData)
        {
            var userDataBucket = configuration.GetSection(ApplicationConstants.ConfigSections.UserDataSection).Value;
            var location = await SaveFile(fileNameWithSuffix, fileData);
            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(location.FileUri, userDataBucket);
            return preSignedUrl;
        }

        private string GetFileSuffix(string fileNameWithExtension)
        {
            var fileName = FileName.ParseFileName(fileNameWithExtension);
            if (!fileName.HasSuffix) throw new DomainException("All files must provide a suffix.");
            return fileName.Suffix;
        }

        public FileLocation GetFileLocation(string fileNameWithSuffix)
        {
            throw new NotImplementedException();
        }
    }

    public class CloudSaverWritesToDatabaseDecorator : ICloudFileSaver
    {
        private readonly ICloudFileSaver cloudFileSaver;
        private readonly IAccountRepository accountRepository;

        public CloudSaverWritesToDatabaseDecorator(ICloudFileSaver cloudFileSaver, IAccountRepository accountRepository)
        {
            this.cloudFileSaver = cloudFileSaver;
            this.accountRepository = accountRepository;
        }

        public async Task<FileLocation> SaveFile(string fileNameWithSuffix, IFormFile fileData)
        {
            var location = await cloudFileSaver.SaveFile(fileNameWithSuffix, fileData);
            var account = await accountRepository.GetAccount();
            account.AccountLogoUri = location.ValueForDatabase;
            await accountRepository.CommitChangesAsync();

            return location;
        }

        public async Task<string> SaveFileAndGetLink(string fileNameWithSuffix, IFormFile fileData)
        {
            return await cloudFileSaver.SaveFileAndGetLink(fileNameWithSuffix, fileData);
        }

        public FileLocation GetFileLocation(string fileNameWithSuffix)
        {
            throw new NotImplementedException();
        }
    }

    public class FileName
    {
        public string FileStem { get; set; }
        public string Suffix { get; set; }

        public bool HasSuffix => !(Suffix is null);

        public static FileName ParseFileName(string fileName)
        {
            var suffix = Path.GetExtension(fileName);
            var stem = Path.GetFileNameWithoutExtension(fileName);
            return new FileName
            {
                FileStem = stem,
                Suffix = suffix
            };
        }

        public static FileName ParseFileNameWithStemOverride(string fileName, string stemOverride)
        {
            var fName = ParseFileName(fileName);
            return new FileName
            {
                FileStem = stemOverride,
                Suffix = fName.Suffix
            };
        }
    }

    public class FileLocation
    {
        public string FileUri { get; set; }
        public CloudType CloudType { get; set; }
        public string ValueForDatabase { get; set; }
    }

    public enum CloudType
    {
        Aws = 0,
        Azure = 1,
        Google = 2
    }
}