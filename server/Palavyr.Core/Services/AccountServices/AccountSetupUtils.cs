using Palavyr.Core.Common.UIDUtils;

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