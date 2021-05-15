using Palavyr.Core.Common.UniqueIdentifiers;

namespace Palavyr.Core.Services.AccountServices
{
    public static class NewAccountUtils
    {
        public static string GetNewAccountId()
        {
            return GuidUtils.CreateShortenedGuid(2);
        }
    }
}