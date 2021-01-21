using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Amazon.S3Services;
using Palavyr.Common.FileSystem;
using Palavyr.Common.FileSystem.FormPaths;

namespace Palavyr.BackupAndRestore.UserData
{
    public interface IUserDataBackup
    {
        Task CreateFullUserDataBackup(string snapshotTimeStamp);
    }

    public class UserDataBackup : IUserDataBackup
    {
        private readonly ILogger<UserDataBackup> logger;
        private readonly S3Saver s3Saver;
        private string UserDataDirectory => Path.Combine(MagicPathStrings.InstallationRoot, MagicPathStrings.UserData);

        public UserDataBackup(ILogger<UserDataBackup> logger, S3Saver s3Saver)
        {
            this.logger = logger;
            this.s3Saver = s3Saver;
        }

        public async Task CreateFullUserDataBackup(string snapshotTimeStamp)
        {
            var snapshotName = $"Palavyr-UserData-{snapshotTimeStamp}";

            var destinationArchiveFileName = Path.Combine(FormDirectoryPaths.FormTempUserDataBackupDirectory(), snapshotName);
            logger.LogDebug($"User Data Backup: {destinationArchiveFileName}");
            ZipFile.CreateFromDirectory(UserDataDirectory, destinationArchiveFileName);

            await s3Saver.SaveZipToS3(destinationArchiveFileName, snapshotTimeStamp, snapshotName);
            
            DiskUtils.DeleteUserDataBackupFolder();
        }
    }
}