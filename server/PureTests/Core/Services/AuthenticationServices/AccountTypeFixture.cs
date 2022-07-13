using Palavyr.Core.Services.AuthenticationServices;
using Shouldly;
using Xunit;

namespace Pure.Core.Services.AuthenticationServices
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