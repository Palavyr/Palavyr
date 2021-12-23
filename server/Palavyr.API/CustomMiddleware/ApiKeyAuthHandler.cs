using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Palavyr.Core.Data;
using Palavyr.Core.GlobalConstants;

namespace Palavyr.API.CustomMiddleware
{
    public class ApiKeyRequirement : IAuthorizationRequirement
    {
    }

    public class ApiKeyAuthSchemeOptions : AuthenticationSchemeOptions
    {
    }


    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthSchemeOptions>
    {
        private readonly AccountsContext accountsContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<ApiKeyAuthenticationHandler> logger;
        private readonly IMediator mediator;

        public ApiKeyAuthenticationHandler(
            AccountsContext accountsContext,
            IHttpContextAccessor contextAccessor,
            IOptionsMonitor<ApiKeyAuthSchemeOptions> options,
            ILoggerFactory loggerFactory,
            ILogger<ApiKeyAuthenticationHandler> logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IMediator mediator) : base(options, loggerFactory, encoder, clock)
        {
            this.accountsContext = accountsContext;
            httpContextAccessor = contextAccessor;
            this.logger = logger;
            this.mediator = mediator;
        }
        
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            logger.LogDebug("Calling Api Authentication handler...");

            // https://widget.palavyr.com/widget?key={apikey} with header  "action": "apiKeyAccess"
            var httpContext = httpContextAccessor.HttpContext; // Access context here

            if (httpContext.Request.Headers[ApplicationConstants.MagicUrlStrings.Action].ToString() != ApplicationConstants.MagicUrlStrings.ApiKeyAccess)
            {
                return await Task.FromResult(AuthenticateResult.Fail("Incorrect action header"));
            }

            var found = httpContext.Request.Query.TryGetValue("key", out var apiKey);
            if (!found)
            {
                return await Task.FromResult(AuthenticateResult.Fail("Could not find the API Key in url."));
            }
            
            var account = accountsContext.Accounts.SingleOrDefault(row => row.ApiKey == apiKey.ToString());
            if (account == null)
            {
                return await Task.FromResult(AuthenticateResult.Fail("Api Key not attached to any accounts."));
            }

            if (!account.Active)
            {
                return await Task.FromResult(AuthenticateResult.Fail(
                    "Account is not activated. Check your email for an activation code to use with the dashboard."));
            }

            await mediator.Publish(new SetAccountEvent(account.AccountId));

            // account found - apikey is legit. Now make a claim ticket...
            var claims = new[] {new Claim(ClaimTypes.SerialNumber, apiKey.ToString())};
            var claimsIdentity = new ClaimsIdentity(claims, nameof(ApiKeyAuthenticationHandler));
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);
            
            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}