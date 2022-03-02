using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Services.TemporaryPaths
{
    public interface ITemporaryPath
    {
        SafeFile CreateLocalTempSafeFile(ExtensionTypes extension = ExtensionTypes.Pdf);
        SafeFile CreateLocalTempSafeFile(string fileStem, ExtensionTypes extension = ExtensionTypes.Pdf);
        S3DownloadFile CreateLocalS3SavePath(string fileName);
        void DeleteLocalTempFile(string fileName);
    }

    public class TemporaryPath : ITemporaryPath
    {
        private readonly ILogger<TemporaryPath> logger;
        private readonly ISafeFileNameCreator safeFileNameCreator;

        public TemporaryPath(ILogger<TemporaryPath> logger, ISafeFileNameCreator safeFileNameCreator)
        {
            this.logger = logger;
            this.safeFileNameCreator = safeFileNameCreator;
        }

        public SafeFile CreateLocalTempSafeFile(ExtensionTypes extension = ExtensionTypes.Pdf)
        {
            var safeFileName = safeFileNameCreator.CreateSafeFileName(extension);
            var isolatedStorageDirectory = IsolatedStorageFile.GetMachineStoreForApplication().GetStorageDirectory();
            return new SafeFile()
            {
                FileStem = safeFileName.Stem,
                FileNameWithExtension = safeFileName.FileNameWithExtension,
                TempDirectory = isolatedStorageDirectory,
                S3Key = Path.Combine(isolatedStorageDirectory, safeFileName.FileNameWithExtension)
            };
        }

        public SafeFile CreateLocalTempSafeFile(string fileNameStem, ExtensionTypes extension = ExtensionTypes.Pdf)
        {
            if (Path.GetExtension(fileNameStem) != null)
            {
                throw new DomainException("An extension-less file name stem must be used with CreateLocalTempSafeFile. An extension may be specified separately");
            }

            
            var isolatedStorageDirectory = IsolatedStorageFile.GetMachineStoreForApplication().GetStorageDirectory();
            var safeFileName = safeFileNameCreator.CreateSafeFileName(fileNameStem, extension);
            return new SafeFile
            {
                FileStem = safeFileName.Stem,
                FileNameWithExtension = safeFileName.FileNameWithExtension,
                TempDirectory = isolatedStorageDirectory,
                S3Key = Path.Combine(isolatedStorageDirectory, safeFileName.FileNameWithExtension)
            };
        }

        public S3DownloadFile CreateLocalS3SavePath(string fileName)
        {
            var isolatedStorageDirectory = IsolatedStorageFile.GetMachineStoreForApplication().GetStorageDirectory();
            return new S3DownloadFile
            {
                FileNameWithExtension = fileName,
                TempDirectory = isolatedStorageDirectory,
                TempFilePath = Path.Combine(isolatedStorageDirectory, fileName)
            };
        }

        public void DeleteLocalTempFile(string fileName)
        {
            var isolatedStorage = IsolatedStorageFile.GetMachineStoreForApplication();
            if (isolatedStorage.FileExists(fileName))
            {
                try
                {
                    isolatedStorage.DeleteFile(fileName);
                }
                catch (IOException)
                {
                    logger.LogDebug($"Unable to delete file: {fileName}");
                    logger.LogDebug($"Full path at: {Path.Combine(isolatedStorage.GetStorageDirectory(), fileName)}");
                    throw;
                }
            }
        }
    }

 
}