using Palavyr.Common.uniqueIdentifiers;

namespace Palavyr.API.controllers.accounts.newAccount
{
    public static class NewAccountUtils
    {
        public static string GetNewAccountId()
        {
            return GuidUtils.CreateShortenedGuid(2);
        }
    }
}