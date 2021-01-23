namespace Palavyr.FileSystem.ExtensionMethods.PathExtensions
{
    public static class PathTypeConversion
    {
        public static string ConvertToUnix(this string path) => path.Replace("\\", "/");
    }
}