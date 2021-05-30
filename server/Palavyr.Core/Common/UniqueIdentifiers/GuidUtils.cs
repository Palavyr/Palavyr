using System.ComponentModel.DataAnnotations;

namespace Palavyr.Core.Common.UniqueIdentifiers
{
    public class GuidUtils : IGuidUtils
    {
        public string CreateShortenedGuid([Range(1, 4)]int take)
        {
            return StaticGuidUtils.CreateShortenedGuid(take);
        }

        public string CreatePseudoRandomString(int take)
        {
            return StaticGuidUtils.CreatePseudoRandomString(take);
        }

        public string CreateNewId()
        {
            return StaticGuidUtils.CreateNewId();
        }
    }
}