using System.IO;
using Palavyr.Core.Common.ExtensionMethods.PathExtensions;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Services.TemporaryPaths;

namespace Palavyr.IntegrationTests.TestUtils
{
    public static class TempS3Utils
    {
        public static string CreateTempS3Key(string filename, ExtensionTypes extension = ExtensionTypes.Png)
        {
            return Path.Join(A.RandomName(), A.RandomName(), string.Join(".", filename, extension.ToString().ToLowerInvariant())).ConvertToUnix();
        }
    }
}