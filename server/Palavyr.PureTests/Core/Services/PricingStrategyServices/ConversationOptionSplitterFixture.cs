using System.Threading.Tasks;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Services.PricingStrategyTableServices;
using Shouldly;
using Test.Common;
using Test.Common.ExtensionsMethods;
using Xunit;

namespace Palavyr.PureTests.Core.Services.PricingStrategyServices
{
    public class ConversationOptionSplitterFixture : IAsyncLifetime, IUnitTestFixture
    {
        private IConversationOptionSplitter splitter;

        [Fact]
        public void WhenAListOfValueOptionsAreProvided_TheyAreJoinedCorrectly()
        {
            var options = new[] { "Option1", "Option2", "Option3" };

            var result = splitter.JoinValueOptions(options);

            result.ShouldBe("Option1|peg|Option2|peg|Option3");
        }

        [Fact]
        public void WhenAListOfOptionValuesAreProvide_AndAnOptionIsEmpty_ThatOptionShouldNotBeIncluded()
        {
            var options = new[] { "Option1", "", "Option3" };

            var result = splitter.JoinValueOptions(options);

            result.ShouldBe("Option1|peg|Option3");
        }

        [Fact]
        public void WhenAnOptionStringIsProvided_ItIsSpitIntoTheCorrectNumberOfElements()
        {
            var options = "Option1|peg|Option2|peg|Option3,Test";

            var result = splitter.SplitValueOptions(options);

            result.ShouldBe(new[] { "Option1", "Option2", "Option3,Test" });
        }

        [Fact]
        public void WhenAGuidIsRequested_AGuidIsReturned()
        {
            var guid = StaticGuidUtils.CreateNewId();
            var sut = "wow-" + guid;
            var result = splitter.GetTableIdFromPricingStrategyNodeType(sut);

            result.ShouldBe(guid);
        }

        [Fact]
        public void WhenAGuidIsRequested_AndNoGuidIsPresent_AnExceptionIsThrown()
        {
            var sut = "WOw-Thisisa-Crazy-Thing";
            var result = splitter.GetTableIdFromPricingStrategyNodeType(sut);
            result.ShouldBeNull();
        }

        public Task InitializeAsync()
        {
            splitter = this.GetNodeSplitter();
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}