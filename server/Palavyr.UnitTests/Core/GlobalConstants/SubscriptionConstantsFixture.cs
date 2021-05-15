using NUnit.Framework;
using Palavyr.Core.GlobalConstants;
using Shouldly;

namespace PalavyrServer.UnitTests.Core.GlobalConstants
{

    [TestFixture(Category = "Authentication")]
    public class SubscriptionConstantsFixture
    {

        [Test]
        public void SubscriptionConstantsAreSetCorrectly()
        {
            ApplicationConstants.SubscriptionConstants.DefaultNumAreas.ShouldBe(2);
            ApplicationConstants.SubscriptionConstants.PremiumNumAreas.ShouldBe(6);
            ApplicationConstants.SubscriptionConstants.ProNumAreas.ShouldBe(99999999);
        }
    }
}