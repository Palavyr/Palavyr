using System.IO;

namespace Palavyr.Common.FileSystem.FormPaths
{
    public static class FormDatabaseBackupPaths
    {
        /// <summary>
        /// $"C:\\PalavyrData\\BackupTempDirectory\\Accounts.sql
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FormDbBackupPath(string fileName)
        {
            var directoryPath = FormDirectoryPaths.FormTempDbBackupDirectory();
            var databaseFilePath = Path.Combine(directoryPath, fileName);
            return databaseFilePath;
        }
    }
}