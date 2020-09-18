using System.IO;
using Palavyr.Common.FileSystem;

namespace Palavyr.API.pathUtils
{
    public static class DiskUtils
    {
        public static bool ValidatePathExists(string path) => File.Exists(path);

        public static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        
        public static void DeleteAreaFolder(string accountId, string areaId)
        {
            var areaDir = FormDirectoryPaths.FormAreaDir(accountId, areaId);
            var dirInfo = new DirectoryInfo(areaDir);
            if (dirInfo.Exists)
            {
                Directory.Delete(areaDir);
            }
        }
    }
}