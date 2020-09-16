using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DashboardServer.API.pathUtils
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
            // TODO ensure the files are present in the FileNameMap DB
            var path = DiskUtils.GetAttachmentsFolderAsDiskPath(accountId, areaId);
            if (!Directory.Exists(path))
                throw new Exception();
            
            var directory = new DirectoryInfo(path);
            return directory.GetFiles("*");
        }

        public static string FormAttachmentPath(string accountId, string areaId, string fileId)
        {
            var attachmentsDir = DiskUtils.GetAttachmentsFolderAsDiskPath(accountId, areaId);
            return Path.Combine(attachmentsDir, fileId);
        }
    }
}