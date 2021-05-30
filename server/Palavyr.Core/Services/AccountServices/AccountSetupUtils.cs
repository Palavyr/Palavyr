using Palavyr.Core.Common.UniqueIdentifiers;

namespace Palavyr.Core.Services.AccountServices
{
    public class NewAccountUtils : INewAccountUtils
    {
        private readonly IGuidUtils guidUtils;

        public NewAccountUtils(IGuidUtils guidUtils)
        {
            this.guidUtils = guidUtils;
        }
        public string GetNewAccountId()
        {
            return guidUtils.CreateShortenedGuid(2);
        }
    }
}