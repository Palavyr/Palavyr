using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.CustomMiddleware.MiddlewareHandlers;
using Palavyr.Core.Data;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.API.CustomMiddleware
{
    public class SetAccountIdContextMiddleware
    {
        private readonly RequestDelegate next;
        private ILogger<SetAccountIdContextMiddleware> logger;

        private Dictionary<string, string> ResponseHeaders = new Dictionary<string, string>
        {
            { "Access-Control-Allow-Origin", "*" }
        };

        public SetAccountIdContextMiddleware(RequestDelegate next, ILogger<SetAccountIdContextMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IWebHostEnvironment env,
            IMediator mediator,
            IEntityStore<Session> sessionStore,
            AccountsContext accountContext,
            IUnitOfWorkContextProvider unitOfWorkContextProvider)
        {
            logger.LogDebug("Settings magic string headers...");

            var action = context.Request.Headers[ApplicationConstants.MagicUrlStrings.Action].ToString();

            if (action == ApplicationConstants.MagicUrlStrings.SessionAction || action == ApplicationConstants.MagicUrlStrings.LogoutAction)
            {
                logger.LogDebug("Session action detected. Searching for the session Id...");
                var sessionId = context.Request.Headers[ApplicationConstants.MagicUrlStrings.SessionId].ToString().Trim();
                if (!string.IsNullOrEmpty(sessionId))
                {
                    logger.LogDebug("Session Id found - performing looking in the persistence store...");
                    var session = await sessionStore.RawReadonlyQuery().SingleOrDefaultAsync(x => x.SessionId == sessionId);
                    if (session != null)
                    {
                        logger.LogDebug("Session found. Assigning account Id to the Request Header.");
                        await mediator.Publish(new SetAccountEvent(session.AccountId), context.RequestAborted);
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
        }
    }
}