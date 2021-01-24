using System.IO;

namespace Palavyr.Common.FileSystem.FormPaths

{
    public static class FormDirectoryPaths
    {
        public static string FormAccountDirWithCreate(string accountId)
        {
            var accountDir = Path.Combine(MagicPathStrings.InstallationRoot, MagicPathStrings.UserData, accountId);
            DiskUtils.CreateDir(accountDir);
            return accountDir;
        }

        public static string FormAttachmentDirectoryWithCreate(string accountId, string areaId)
        {
            var attachmentDir = Path.Combine(FormAccountDirWithCreate(accountId), MagicPathStrings.AreaData, areaId, MagicPathStrings.Attachments);
            DiskUtils.CreateDir(attachmentDir);
            return attachmentDir;
        }

        public static string FormResponsePdfDirWithCreate(string accountId)
        {
            var responseDir = Path.Combine(FormAccountDirWithCreate(accountId), MagicPathStrings.PreviewPDF);
            DiskUtils.CreateDir(responseDir);
            return responseDir;
        }

        public static string FormAreaDir(string accountId, string areaId)
        {
            var accountDir = FormAccountDirWithCreate(accountId);
            var areaDir = Path.Combine(accountDir, MagicPathStrings.AreaData, areaId);
            DiskUtils.CreateDir(areaDir);
            return areaDir;
        }

        public static string FormLogoImageDir(string accountId)
        {
            var logoImageDir = Path.Combine(FormAccountDirWithCreate(accountId), MagicPathStrings.Logo);
            DiskUtils.CreateDir(logoImageDir);
            return logoImageDir;
        }

        public static string FormTempDbBackupDirectory()
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