using System.ComponentModel.DataAnnotations;

namespace Palavyr.Core.Common.UniqueIdentifiers
{
    public interface IGuidUtils
    {
        string CreateShortenedGuid([Range(1, 4)] int take);
        string CreateShortenedGuid();
        string CreatePseudoRandomString(int take);
        string CreateNewId();
    }
}