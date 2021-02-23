using Palavyr.Common.UIDUtils;

namespace Palavyr.Services.AccountServices
{
    public static class NewAccountUtils
    {
        public static string GetNewAccountId()
        {
            return GuidUtils.CreateShortenedGuid(2);
        }
    }
}