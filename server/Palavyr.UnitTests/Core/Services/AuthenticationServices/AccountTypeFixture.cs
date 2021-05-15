using NUnit.Framework;
using Palavyr.Core.Services.AuthenticationServices;
using Shouldly;

namespace PalavyrServer.Tests.Core.Services.AuthenticationServices
{
    [TestFixture(Category = "Authentication")]
    public class AccountTypeFixture
    {

        [Test]
        public void AccountEnumIsOrderedCorrectly()
        {
            ((int)AccountType.Default).ShouldBe(0);
            ((int)AccountType.Google).ShouldBe(1);
        }
    }
}