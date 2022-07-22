using Palavyr.Core.Services.AuthenticationServices;
using Shouldly;
using Xunit;

namespace Palavyr.PureTests.Core.Services.AuthenticationServices
{
    [Trait("Authentication", "Account Type")]
    public class AccountTypeFixture
    {

        [Fact]
        public void AccountEnumIsOrderedCorrectly()
        {
            ((int)AccountType.Default).ShouldBe(0);
        }
    }
}