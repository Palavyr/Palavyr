using System.IO;
using Palavyr.API.pathUtils;

namespace Palavyr.Common.FileSystem

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
        
        
    }
}