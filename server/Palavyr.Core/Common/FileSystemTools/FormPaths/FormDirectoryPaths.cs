using System.IO;

namespace Palavyr.Core.Common.FileSystemTools.FormPaths

{
    public static class FormDirectoryPaths
    {
        public static string FormTempDbBackupDirectory() // DO NOT DELETE
        {
            var backupDirName = "DbBackupTempDirectory";
            var backupDir = Path.Combine(MagicPathStrings.InstallationRoot, backupDirName);
            DiskUtils.CreateDir(backupDir);
            return backupDir;
        }

        public static string FormZippableDbBackupDirectory()
        {
            var zippableDirName = "Palavyr-Db-Backup";
            var zippableDirectory = Path.Combine(FormTempDbBackupDirectory(), zippableDirName);
            DiskUtils.CreateDir(zippableDirectory);
            return zippableDirectory;
        }

        public static string FormTempUserDataBackupDirectory()
        {
            var backupDirName = "UserDataBackupTempDirectory";
            var backupDir = Path.Combine(MagicPathStrings.InstallationRoot, backupDirName);
            DiskUtils.CreateDir(backupDir);
            return backupDir;
        }

        public static string FormLocalRestoreDirectory()
        {
            var restoreDirName = "TempRestoreDirectory";
            var tempRestoreDirectory = Path.Combine(MagicPathStrings.InstallationRoot, restoreDirName);
            DiskUtils.CreateDir(tempRestoreDirectory);
            return tempRestoreDirectory;
        }
    }
}