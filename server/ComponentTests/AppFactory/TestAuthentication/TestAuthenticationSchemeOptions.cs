using Microsoft.AspNetCore.Authentication;

namespace Component.AppFactory.TestAuthentication
{
    public class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string Role { get; set; }
    }
}