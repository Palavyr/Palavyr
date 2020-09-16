using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DashboardServer.API.pathUtils
{
    public static class DiskUtils
    {
        public static bool ValidatePathExists(string path) => File.Exists(path);

        public static string CreatePreviewLinkAsDiskPath(string accountId, string fileName) => 
            Path.Combine(MagicString.InstallationRoot, MagicString.UserData, accountId,MagicString.PreviewPDF, fileName);

        public static string GetUserFolderAsDiskPath(string accountId)
        {
            var accountFolder = Path.Combine(MagicString.InstallationRoot, MagicString.UserData, accountId);
            if (!Directory.Exists(accountFolder))
                Directory.CreateDirectory(accountFolder);
            return accountFolder;
        }

        public static string GetAreaFolderAsDiskPath(string accountId, string areaId)
        {
            var areaFolder = Path.Combine(MagicString.InstallationRoot, MagicString.UserData, accountId, MagicString.AreaData, areaId);
            if (!Directory.Exists(areaFolder))
                Directory.CreateDirectory(areaFolder);
            return areaFolder;
        }
        
        public static string GetAttachmentsFolderAsDiskPath(string accountId, string areaId)
        {
            var attachmentFolder = Path.Combine(MagicString.InstallationRoot, MagicString.UserData, accountId, MagicString.AreaData, areaId, MagicString.Attachments);
            if (!Directory.Exists(attachmentFolder))
                Directory.CreateDirectory(attachmentFolder);
            return attachmentFolder;
        }

        public static void DeleteAreaFolder(string accountId, string areaId)
        {
            //C:\PalavyrData\UserData\dashboardDev\AreaData
            var areaFolderPath = Path.Combine(MagicString.InstallationRoot, MagicString.UserData, accountId,
                MagicString.AreaData, areaId);
            var dirInfo = new DirectoryInfo(areaFolderPath);
            if (dirInfo.Exists)
            {
                Directory.Delete(areaFolderPath);
            }
        }
    }
}