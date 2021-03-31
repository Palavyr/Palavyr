namespace Palavyr.Core.Common.ExtensionMethods.PathExtensions
{
    public static class PathTypeConversion
    {
        public static string ConvertToUnix(this string path) => path.Replace("\\", "/");
    }
}