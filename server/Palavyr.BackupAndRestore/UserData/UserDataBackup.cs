using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Amazon;
using Palavyr.Amazon.S3Services;
using Palavyr.Common.FileSystem;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.FileSystem.UIDUtils;

namespace Palavyr.BackupAndRestore.UserData
{
    public interface IUserDataBackup
    {
        Task CreateFullUserDataBackup(TimeUtils timeStamp);
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

        public async Task CreateFullUserDataBackup(TimeUtils timeStamp)
        {
            var localZipFilePath = LocalPathUtils.FormLocalTempDbZipFilePath(timeStamp);
            
            logger.LogDebug($"User Data Backup: {localZipFilePath}");
            ZipFile.CreateFromDirectory(UserDataDirectory, localZipFilePath);

            var fileKey = AmazonPathUtils.FormS3UserDataBackupKey(timeStamp);
            await s3Saver.SaveZipToS3(AmazonConstants.ArchivesBucket, localZipFilePath, fileKey);
            
            DiskUtils.DeleteUserDataBackupFolder();
        }
        
        
    }
}