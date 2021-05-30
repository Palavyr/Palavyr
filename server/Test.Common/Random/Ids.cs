using Palavyr.Core.Common.UniqueIdentifiers;

namespace Test.Common.Random
{
    public static class A
    {
        public static string RandomName()
        {
            return StaticGuidUtils.CreateShortenedGuid(1);
        }
    }
}