using Palavyr.Core.GlobalConstants;
using Shouldly;
using Xunit;

namespace PalavyrServer.UnitTests.Core.GlobalConstants
{
    [Trait("Constants ", "Subscriptions")]
    public class SubscriptionConstantsFixture
    {
        [Fact]
        public void SubscriptionConstantsAreSetCorrectly()
        {
            ApplicationConstants.SubscriptionConstants.DefaultNumAreas.ShouldBe(2);
            ApplicationConstants.SubscriptionConstants.PremiumNumAreas.ShouldBe(6);
            ApplicationConstants.SubscriptionConstants.ProNumAreas.ShouldBe(99999999);
        }
    }
}