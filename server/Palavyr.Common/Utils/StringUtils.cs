using System.Text.RegularExpressions;

namespace Palavyr.Common.Utils
{
    public static class StringUtils
    {
        public static string[] SplitCamelCase(string source) {
            return Regex.Split(source, @"(?<!^)(?=[A-Z])");
        }
    }
}