using NUnit.Framework;
using Palavyr.FileSystem.UniqueIdentifiers;

namespace PalavyrServer.Tests.Palavyr.Common.UniqueIdentifiers
{
    [TestFixture]
    public class TimeUtilsFixture
    {
        [Test]
        public void TestDateTimeFormat()
        {
            var format = TimeUtils.DateTimeFormat;
            Assert.AreEqual(format, "yyyy-dd-M--HH-mm-ss");
        }
    }
}