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

        public static string FormResponsePDFDirWithCreate(string accountId)
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
            var backupDirName = "BackupTempDirectory";
            var backupDir = Path.Combine(MagicPathStrings.InstallationRoot, backupDirName);
            DiskUtils.CreateDir(backupDir);
            return backupDir;
        }
    }
}