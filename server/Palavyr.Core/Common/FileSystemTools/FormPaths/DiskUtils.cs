﻿using System.IO;

namespace Palavyr.Core.Common.FileSystemTools.FormPaths
{
    public static class DiskUtils
    {
        public static bool ValidatePathExists(string path) => File.Exists(path);

        public static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void DeleteDbBackupFolder()
        {
            DeleteFolder(FormDirectoryPaths.FormTempDbBackupDirectory());
        }

        public static void DeleteUserDataBackupFolder()
        {
            DeleteFolder(FormDirectoryPaths.FormTempUserDataBackupDirectory());
        }

        public static void DeleteTempDirectory(string tempDirectory)
        {
            DeleteFolder(tempDirectory);
        }

        /// <summary>
        /// This will recursively delete a directory (and thus with its contents)
        /// If file permissions are restricted on the directory or any of its subdir contents,
        /// this may throw an error
        /// </summary>
        /// <param name="directoryPath"></param>
        private static void DeleteFolder(string directoryPath)
        {
            var dirInfo = new DirectoryInfo(directoryPath);
            if (dirInfo.Exists)
            {
                Directory.Delete(directoryPath, true);
            }
        }
    }
}