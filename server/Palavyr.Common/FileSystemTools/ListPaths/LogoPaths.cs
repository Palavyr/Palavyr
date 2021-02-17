using System.Collections.Generic;
using System.IO;
using System.Linq;
using Palavyr.Common.FileSystem.FormPaths;

namespace Palavyr.Common.FileSystem.ListPaths
{
    public static class LogoPaths
    {
        public static List<string> ListLogoPathsAsDiskPaths(string accountId)
        {
            var files = GetLogoFileList(accountId);
            return files.Select(x => x.FullName).ToList();
        }

        public static FileInfo[] GetLogoFileList(string accountId)
        {
            var logoDir = FormDirectoryPaths.FormLogoImageDir(accountId);
            var directory = new DirectoryInfo(logoDir);
            return directory.GetFiles("*");
        }
    }
}