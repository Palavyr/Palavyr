using System.IO;
using Palavyr.Amazon;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.FileSystem.UIDUtils;

namespace Palavyr.BackupAndRestore
{
    public class LocalPathUtils
    {
        public static string FormLocalTempDbZipFilePath(TimeUtils timeStamp)
        {
            var fileName = string.Join(".", new[] { "Palavyr", AmazonConstants.Databases, timeStamp.SecondPrecision, "zip"});
            return Path.Combine(FormDirectoryPaths.FormTempDbBackupDirectory(), fileName);
        }

        public static string FormLocalTempDbExportPath(string databaseName, TimeUtils timeStamp)
        {
            var outfileName = string.Join(".", new[] {databaseName, timeStamp.SecondPrecision, "sql"});
            return FormDatabaseBackupPaths.FormZippableDbBackupPath(outfileName);
        }
    }
}