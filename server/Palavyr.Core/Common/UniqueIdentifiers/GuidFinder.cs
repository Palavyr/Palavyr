using System;
using System.Text.RegularExpressions;

namespace Palavyr.Core.Common.UniqueIdentifiers
{
    public class GuidFinder
    {
        // assumes only 1 guid present in string
        string GUIDPattern = @"[{(]?\b[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}\b[)}]?";

        public string FindGuid(string stringWithSingleGuid)
        {
            var result = MatchGuid(stringWithSingleGuid);
            if (!string.IsNullOrWhiteSpace(result))
            {
                return result;
            }

            throw new Exception($"GUID was not found in the string: {stringWithSingleGuid}");
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

        string MatchGuid(string input)
        {
            return Regex.Match(input, GUIDPattern, RegexOptions.IgnoreCase).Value;
        }
    }
}