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
        public static string FormZippableDbBackupPath(string fileName)
        {
            var zippablePath = FormDirectoryPaths.FormZippableDbBackupDirectory();
            return Path.Combine(zippablePath, fileName);
        }
    }
}