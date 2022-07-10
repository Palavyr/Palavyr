using System.Linq;

namespace Palavyr.Core.Common.ExtensionMethods.PathExtensions
{
    public static class PathTypeConversion
    {
        public static string ConvertToUnix(this string path) => path.Replace("\\", "/");
    }

    public static class UriUtils
    {
        public static string Join(params string[] components)
        {
            var trimmable = @"/\".ToCharArray();
            var cleaned = components
                .Select(component => component.TrimStart(trimmable).TrimEnd(trimmable));
            return string.Join("/", cleaned);
        }
    }
}