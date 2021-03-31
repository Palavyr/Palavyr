using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.BackupAndRestore.Paths;
using Palavyr.Core.Common.FileSystemTools;
using Palavyr.Core.Common.FileSystemTools.FormPaths;
using Palavyr.Core.Common.UIDUtils;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.BackupAndRestore.UserData
{
    public interface IUserDataBackup
    {
        Task<string> CreateFullUserDataBackup(TimeUtils timeStamp, string bucket);
    }

    public class UserDataBackup : IUserDataBackup
    {
        private readonly ILogger<UserDataBackup> logger;
        private readonly IS3Saver s3Saver;
        private string UserDataDirectory => Path.Combine(MagicPathStrings.InstallationRoot, MagicPathStrings.UserData);

        public UserDataBackup(ILogger<UserDataBackup> logger, IS3Saver s3Saver)
        {
            this.logger = logger;
            this.s3Saver = s3Saver;
        }

        public async Task<string> CreateFullUserDataBackup(TimeUtils timeStamp, string bucket)
        {
            var localZipFilePath = LocalPathUtils.FormLocalTempUserDataZipFilePath(timeStamp);

            logger.LogDebug($"User Data Backup: {localZipFilePath}");
            ZipFile.CreateFromDirectory(UserDataDirectory, localZipFilePath);

            var fileKey = AmazonPathUtils.FormS3UserDataBackupKey(timeStamp);
            await s3Saver.SaveZipToS3(bucket, localZipFilePath, fileKey);

            DiskUtils.DeleteUserDataBackupFolder();
            return fileKey;
        }
    }
}