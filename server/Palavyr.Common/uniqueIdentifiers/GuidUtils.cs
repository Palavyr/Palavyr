using System;
using System.Linq;

namespace Palavyr.Common.uniqueIdentifiers
{
    public static class GuidUtils
    {
        public static string CreateShortenedGuid(int take)
        {
            var useNumParts = (take > 4) ? 4 : take;
            var ids = Guid.NewGuid().ToString().Split('-').Take(useNumParts).ToList();
            return string.Join('-', ids);
        }
    }
}