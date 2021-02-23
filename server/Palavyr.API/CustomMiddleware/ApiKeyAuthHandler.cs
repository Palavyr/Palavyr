using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Palavyr.Common.RequestsTools;
using Palavyr.Data;

namespace Palavyr.API.CustomMiddleware
{
    public class ApiKeyRequirement : IAuthorizationRequirement
    {
    }

    public class ApiKeyAuthSchemeOptions : AuthenticationSchemeOptions
    {
    }


    public class ApiKeyAuthenticationHandler
        : AuthenticationHandler<ApiKeyAuthSchemeOptions>
    {
        private readonly AccountsContext _accountsContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiKeyAuthenticationHandler(
            AccountsContext accountsContext,
            IHttpContextAccessor contextAccessor,
            IOptionsMonitor<ApiKeyAuthSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _accountsContext = accountsContext;
            _httpContextAccessor = contextAccessor;
        }
        
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            
            // https://widget.palavyr.com/widget?key={apikey} with header  "action": "apiKeyAccess"
            var httpContext = _httpContextAccessor.HttpContext; // Access context here

            if (httpContext.Request.Headers[MagicUrlStrings.Action].ToString() != MagicUrlStrings.ApiKeyAccess)
            {
                return Task.FromResult(AuthenticateResult.Fail("Incorrect action header"));
            }

            var found = httpContext.Request.Query.TryGetValue("key", out var apiKey);
            if (!found)
            {
                return Task.FromResult(AuthenticateResult.Fail("Could not find API Key in url. Check formatting."));
            }
            
            var account = _accountsContext.Accounts.SingleOrDefault(row => row.ApiKey == apiKey.ToString());
            if (account == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Api Key not attached to any accounts."));
            }

            if (!account.Active)
            {
                return Task.FromResult(AuthenticateResult.Fail(
                    "Account is not activated. Check your email for an activation code to use with the dashboard."));
            }
            
            // set the account id in the header
            httpContext.Request.Headers[MagicUrlStrings.AccountId] = account.AccountId;
            
            // account found - apikey is legit. Now make a claim ticket...
            var claims = new[] {new Claim(ClaimTypes.SerialNumber, apiKey.ToString())};
            var claimsIdentity = new ClaimsIdentity(claims, nameof(ApiKeyAuthenticationHandler));
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);
            
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}