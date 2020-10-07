using NUnit.Framework;
using Palavyr.Common.uniqueIdentifiers;

namespace PalavyrServer.Tests.common.uniqueIdentifiers
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