using Palavyr.Core.Common.UniqueIdentifiers;
using Shouldly;
using Xunit;

namespace PalavyrServer.UnitTests.Common.UniqueIdentifiers
{
    [Trait("Utils", "Time")]
    public class TimeUtilsFixture
    {
        [Fact]
        public void TestDateTimeFormat()
        {
            var format = TimeUtils.DateTimeFormat;
            format.ShouldBe("yyyy-dd-M--HH-mm-ss");
        }
    }
}