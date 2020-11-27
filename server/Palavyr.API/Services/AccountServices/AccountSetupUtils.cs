using Palavyr.FileSystem.UniqueIdentifiers;

namespace Palavyr.API.Services.AccountServices
{
    public static class NewAccountUtils
    {
        public static string GetNewAccountId()
        {
            return GuidUtils.CreateShortenedGuid(2);
        }
    }
}