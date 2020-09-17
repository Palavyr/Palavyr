using System;
using System.Linq;
using Palavyr.Common.uniqueIdentifiers;

namespace Palavyr.API.Controllers
{
    public static class NewAccountUtils
    {
        public static string GetNewAccountId()
        {
            return GuidUtils.CreateShortenedGuid(2);
        }
    }
}