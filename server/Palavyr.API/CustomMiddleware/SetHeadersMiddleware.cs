using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Common.RequestsTools;
using Palavyr.Data;

namespace Palavyr.API.CustomMiddleware
{
    public class SetHeadersMiddleware
    {
        private readonly RequestDelegate next;
        private ILogger<SetHeadersMiddleware> logger;

        public SetHeadersMiddleware(RequestDelegate next, ILogger<SetHeadersMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env, AccountsContext accountContext)
        {
            
            logger.LogDebug("Settings magic string headers...");
            var action = context.Request.Headers[MagicUrlStrings.Action].ToString();

            if (action == MagicUrlStrings.SessionAction)
            {
                logger.LogDebug("Session action detected. Searching for the session Id...");
                var sessionId = context.Request.Headers[MagicUrlStrings.SessionId].ToString();
                if (!string.IsNullOrWhiteSpace(sessionId))
                {
                    logger.LogDebug("Session Id found - performing looking in the persistence store...");
                    var session = accountContext.Sessions.SingleOrDefault(row => row.SessionId == sessionId);
                    if (session != null)
                    {
                        logger.LogDebug("Session found. Assigning account Id to the Request Header.");
                        context.Request.Headers[MagicUrlStrings.AccountId] = session.AccountId;
                    }
                }
            }

            await next(context);
            
            
        }
    }
}