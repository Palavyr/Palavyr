using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.GlobalConstants;

namespace Palavyr.API.CustomMiddleware
{
    public class SetHeadersMiddleware
    {
        private readonly RequestDelegate next;
        private ILogger<SetHeadersMiddleware> logger;

        private Dictionary<string, string> ResponseHeaders = new Dictionary<string, string>
        {
            {"Access-Control-Allow-Origin", "*"}
        };

        public SetHeadersMiddleware(RequestDelegate next, ILogger<SetHeadersMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env, AccountsContext accountContext)
        {
            logger.LogDebug("Settings magic string headers...");
            var action = context.Request.Headers[ApplicationConstants.MagicUrlStrings.Action].ToString();

            if (action == ApplicationConstants.MagicUrlStrings.SessionAction)
            {
                logger.LogDebug("Session action detected. Searching for the session Id...");
                var sessionId = context.Request.Headers[ApplicationConstants.MagicUrlStrings.SessionId].ToString();
                if (!string.IsNullOrWhiteSpace(sessionId))
                {
                    logger.LogDebug("Session Id found - performing looking in the persistence store...");
                    var session = accountContext.Sessions.SingleOrDefault(row => row.SessionId == sessionId);
                    if (session != null)
                    {
                        logger.LogDebug("Session found. Assigning account Id to the Request Header.");
                        context.Request.Headers[ApplicationConstants.MagicUrlStrings.AccountId] = session.AccountId;
                    }
                }
            }

            context.Response.OnCompleted(
                async o =>
                {
                    if (o is HttpContext ctx)
                    {
                        foreach (var (key, value) in ResponseHeaders)
                        {
                            context.Request.Headers[key] = value;
                            logger.LogDebug($"Adding Header: {key} with value: {value}");
                        }
                    }
                }, context);

            await next(context);
            Console.WriteLine("On the way out!");
        }
    }
}