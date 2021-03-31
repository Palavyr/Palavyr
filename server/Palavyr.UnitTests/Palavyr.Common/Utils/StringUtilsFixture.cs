using FluentAssertions;
using NUnit.Framework;
using Palavyr.Core.Common.Utils;

namespace PalavyrServer.Tests.Palavyr.Common.Utils
{
    [TestFixture]
    public class StringUtilsFixture
    {
        [Test]
        public void SplitCamelCase_SplitsOnUpperCase_ReturnsArray()
        {
            var subject = " ThisIsATest";
            var result = StringUtils.SplitCamelCaseAsArray(subject);
            result.Should().Equal(new[] {"This", "Is", "A", "Test"});
        }

        [Test]
        public void SplitCamelCase_SplitsOnUpperCase_ReturnsString()
        {
            var subject = " ThisIsATest";
            var result = StringUtils.SplitCamelCaseAsString(subject);
            result.Should().Equals("This Is A Test");
        }
    }
}