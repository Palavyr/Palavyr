using NUnit.Framework;
using Palavyr.Core.Common.UniqueIdentifiers;

namespace PalavyrServer.UnitTests.Common.UniqueIdentifiers
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