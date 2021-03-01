using Microsoft.AspNetCore.Authentication;

namespace Palavyr.IntegrationTests.TestAuthentication
{
    public class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string Role { get; set; }
    }
}