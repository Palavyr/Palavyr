using Palavyr.Core.Common.UniqueIdentifiers;
using Shouldly;
using Test.Common;
using Test.Common.ExtensionsMethods;
using Xunit;

namespace Pure.Common.UniqueIdentifiers
{
    public class GuidUtilsFixture : IUnitTestFixture
    {
        [Fact]
        public void WhenSingleGuidIsPresent_SingleGuidIsFound()
        {
            var guid = StaticGuidUtils.CreateNewId();
            var test = $"wow-thisisas-{guid}";
            this.GetGuidFinder().FindFirstGuidSuffixOrNull(test).ShouldBe(guid);
        }

        [Fact]
        public void WhenMultipleGuidIsPresent_FirstGuidIsReturned()
        {
            var guid = StaticGuidUtils.CreateNewId();
            var guidTwo = StaticGuidUtils.CreateNewId();
            var test = $"{guid}-wow-{guidTwo}";
            var result= this.GetGuidFinder().FindFirstGuidSuffixOrNull(test);
            result.ShouldBe(guid);
        }

        [Fact]
        public void WhenSingleGuidIsPresent_GuidIsRemoved()
        {
            var test = $"wow-{StaticGuidUtils.CreateNewId()}";
            this.GetGuidFinder().RemoveGuid(test).ShouldBe("wow");
        }

        [Fact]
        public void WhenSingleGuidISPresent_AndNoSeparatorIsProvided_GuidShouldNotBeRemoved()
        {
            var test = $"wow{StaticGuidUtils.CreateNewId()}";
            this.GetGuidFinder().RemoveGuid(test).ShouldBe(test);
        }

        [Fact]
        public void WhenMultipleGuidIsPresent_FirstGuidIsRemoved()
        {
            var guidOne = StaticGuidUtils.CreateNewId();
            var guidTwo = StaticGuidUtils.CreateNewId();
            
            var test = $"wow-{guidOne}-{guidTwo}";
            this.GetGuidFinder().RemoveGuid(test).ShouldBe($"wow-{guidTwo}");
            
        }
    }
}