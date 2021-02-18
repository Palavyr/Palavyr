using System.Text.RegularExpressions;

namespace Palavyr.Common.Utils
{
    public static class StringUtils
    {
        public const string UppercaseCharacterPatter = @"(?<!^)(?=[A-Z])";
        public static string[] SplitCamelCaseAsArray(string str) {
            return Regex.Split(str.Trim(), UppercaseCharacterPatter);
        }

        public static string SplitCamelCaseAsString(string str)
        {
            return string.Join(" ", Regex.Split(str.Trim(), UppercaseCharacterPatter));
        }
    }
}