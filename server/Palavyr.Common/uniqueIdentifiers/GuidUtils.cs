using System;
using System.Linq;

namespace Palavyr.FileSystem.UniqueIdentifiers
{
    public static class GuidUtils
    {
        public static string CreateShortenedGuid(int take)
        {
            var useNumParts = (take > 4) ? 4 : take;
            var ids = Guid.NewGuid().ToString().Split('-').Take(useNumParts).ToList();
            return string.Join('-', ids);
        }

        public static string CreatePseudoRandomString(int take)
        {
            var ids = string.Join("", Guid.NewGuid().ToString().Split('-'));
            var chars = string.Join("", ids.ToCharArray().Take(5));
            return chars;
        }
    }
}