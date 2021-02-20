using System.IO;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.Common.UIDUtils;
using Palavyr.Services.Amazon;

namespace Palavyr.BackupAndRestore.Paths
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
            var outfileName = string.Join(".", new[] {databaseName, "sql"});
            return FormDatabaseBackupPaths.FormZippableDbBackupPath(outfileName);
        }

        public static string FormLocalTempUserDataZipFilePath(TimeUtils timeStamp)
        {
            var outfile = string.Join(".", new[] {DatabaseConstants.CompanyName, "UserData", timeStamp.SecondPrecision, "zip"});
            return FormUserDataBackupPaths.FormZippableUserDataBackupPath(outfile);
        }
    }
}