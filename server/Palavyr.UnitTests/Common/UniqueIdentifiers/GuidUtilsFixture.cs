using Palavyr.Core.Common.UniqueIdentifiers;
using Shouldly;
using Test.Common;
using Test.Common.ExtensionsMethods;
using Xunit;

namespace PalavyrServer.UnitTests.Common.UniqueIdentifiers
{
    public class GuidUtilsFixture : IUnitTestFixture
    {
        [Fact]
        public void WhenSingleGuidIsPresent_SingleGuidIsFound()
        {
            var guid = GuidUtils.CreateNewId();
            var test = $"wow-thisisas-{guid}";
            this.GetGuidFinder().FindFirstGuidSuffix(test).ShouldBe(guid);
        }

        [Fact]
        public void WhenMultipleGuidIsPresent_FirstGuidIsReturned()
        {
            var guid = GuidUtils.CreateNewId();
            var guidTwo = GuidUtils.CreateNewId();
            var test = $"{guid}-wow-{guidTwo}";
            var result= this.GetGuidFinder().FindFirstGuidSuffix(test);
            result.ShouldBe(guid);
        }

        [Fact]
        public void WhenSingleGuidIsPresent_GuidIsRemoved()
        {
            var test = $"wow-{GuidUtils.CreateNewId()}";
            this.GetGuidFinder().RemoveGuid(test).ShouldBe("wow");
        }

        [Fact]
        public void WhenSingleGuidISPresent_AndNoSeparatorIsProvided_GuidShouldNotBeRemoved()
        {
            var test = $"wow{GuidUtils.CreateNewId()}";
            this.GetGuidFinder().RemoveGuid(test).ShouldBe(test);
        }

        [Fact]
        public void WhenMultipleGuidIsPresent_FirstGuidIsRemoved()
        {
            var guidOne = GuidUtils.CreateNewId();
            var guidTwo = GuidUtils.CreateNewId();
            
            var test = $"wow-{guidOne}-{guidTwo}";
            this.GetGuidFinder().RemoveGuid(test).ShouldBe($"wow-{guidTwo}");
            
        }
    }
}