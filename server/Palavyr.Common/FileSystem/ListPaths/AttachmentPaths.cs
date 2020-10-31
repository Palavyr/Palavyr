using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Palavyr.Common.FileSystem.FormPaths
{
    public static class AttachmentPaths
    {
        public static List<string> ListAttachmentsAsDiskPaths(string accountId, string areaId)
        {
            var files = GetAttachmentFileList(accountId, areaId);
            return files.Select(x => x.FullName).ToList();
        }

        public static FileInfo[] GetAttachmentFileList(string accountId, string areaId)
        {
            var attachmentDir = FormDirectoryPaths.FormAttachmentDirectoryWithCreate(accountId, areaId);
            var directory = new DirectoryInfo(attachmentDir);
            return directory.GetFiles("*.pdf");
        }
    }
}