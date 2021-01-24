using System.IO;
using Palavyr.FileSystem.ExtensionMethods.PathExtensions;
using Palavyr.FileSystem.UIDUtils;

namespace Palavyr.Amazon
{
    public static class AmazonPathUtils
    {
        // TODO: make final s3 dir Day Precision.
        public static string FormS3DatabaseBackupKey(TimeUtils timeStamp)
        {
            var fileName = string.Join(".", new[] {AmazonConstants.Databases, timeStamp.SecondPrecision, "zip"});
            return Path.Combine(AmazonConstants.SnapshotsDir, timeStamp.SecondPrecision, fileName).ConvertToUnix();
        }

        public static string FormS3UserDataBackupKey(TimeUtils timeStamp)
        {
            var fileName = string.Join(".", new[] {AmazonConstants.UserData, timeStamp.SecondPrecision, "zip"});
            return Path.Combine(AmazonConstants.SnapshotsDir, timeStamp.SecondPrecision, fileName).ConvertToUnix();
        }
    }
}