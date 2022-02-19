using Palavyr.Core.Common.UniqueIdentifiers;

namespace Test.Common.Random
{
    public static class A
    {
        public static string RandomName()
        {
            return StaticGuidUtils.CreateShortenedGuid(1);
        }

        public static string RandomAccount()
        {
            return StaticGuidUtils.CreateShortenedGuid(1);
        }

        public static string RandomId()
        {
            return StaticGuidUtils.CreateNewId();
        }

        public static string RandomString()
        {
            return $"{StaticGuidUtils.CreatePseudoRandomString(2)} {StaticGuidUtils.CreatePseudoRandomString(2)}";
        }
    }
}