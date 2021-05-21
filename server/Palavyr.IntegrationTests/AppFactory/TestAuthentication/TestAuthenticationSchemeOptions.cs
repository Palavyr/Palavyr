using Microsoft.AspNetCore.Authentication;

namespace Palavyr.IntegrationTests.AppFactory.TestAuthentication
{
    public class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string Role { get; set; }
    }
}