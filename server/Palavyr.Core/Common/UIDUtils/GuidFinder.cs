using System;
using System.Text.RegularExpressions;

namespace Palavyr.Core.Common.UIDUtils
{
    public class GuidFinder
    {
        // assumes only 1 guid present in string
        string GUIDPattern = @"[{(]?\b[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}\b[)}]?";

        public string FindGuid(string stringWithSingleGuid)
        {
            var tableId = Regex.Match(stringWithSingleGuid, GUIDPattern, RegexOptions.IgnoreCase).Value;
            if (!string.IsNullOrWhiteSpace(tableId))
            {
                return tableId;
            }

            throw new Exception($"GUID was not found in the string: {stringWithSingleGuid}");
        }
    }
}