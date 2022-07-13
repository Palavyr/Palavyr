using Microsoft.AspNetCore.Authentication;

namespace IntegrationTests.AppFactory.TestAuthentication
{
    public class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string Role { get; set; }
    }
}