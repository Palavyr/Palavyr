using System.Text.RegularExpressions;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Common.UniqueIdentifiers
{
    public interface IGuidFinder
    {
        string? FindFirstGuidSuffixOrNull(string stringWithSingleGuid);
        void AssertGuidPreset(string stringWithSingleGuid);
        string RemoveGuid(string stringWithSingleGuid);
    }

    public class GuidFinder : IGuidFinder
    {
        // assumes only 1 guid present in string
        string GUIDPattern = @"[-]?[{(]?\b[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}\b[)}]?";

        public string? FindFirstGuidSuffixOrNull(string stringWithSingleGuid)
        {
            var result = MatchGuid(stringWithSingleGuid);
            if (!string.IsNullOrWhiteSpace(result))
            {
                return result.TrimStart('-');
            }

            return null;
        }

        public void AssertGuidPreset(string stringWithSingleGuid)
        {
            var result = FindFirstGuidSuffixOrNull(stringWithSingleGuid);
            if (result is null) throw new GuidNotFoundException($"GUID was not found in the string: {stringWithSingleGuid}");
        }

        public string RemoveGuid(string stringWithSingleGuid)
        {
            var cleanString = stringWithSingleGuid;
            var result = MatchGuid(stringWithSingleGuid);
            if (!string.IsNullOrWhiteSpace(result))
            {
                cleanString = cleanString.Replace(result, "").TrimEnd('-');
            }

            return cleanString;
        }

        private string MatchGuid(string input)
        {
            return Regex.Match(input, GUIDPattern, RegexOptions.IgnoreCase).Value;
        }
    }
}