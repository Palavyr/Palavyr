using Palavyr.Core.Services.AuthenticationServices;
using Shouldly;
using Xunit;

namespace PalavyrServer.UnitTests.Core.Services.AuthenticationServices
{
    [Trait("Authentication", "Account Type")]
    public class AccountTypeFixture
    {

        [Fact]
        public void AccountEnumIsOrderedCorrectly()
        {
            ((int)AccountType.Default).ShouldBe(0);
            ((int)AccountType.Google).ShouldBe(1);
        }
    }
}