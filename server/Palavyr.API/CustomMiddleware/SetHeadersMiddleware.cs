using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Repositories;
using Palavyr.Core.Sessions;

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

        public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env, AccountsContext accountContext, IMediator mediator, IAccountRepository accountRepository)
        {
            logger.LogDebug("Settings magic string headers...");
            var action = context.Request.Headers[ApplicationConstants.MagicUrlStrings.Action].ToString();

            if (action == ApplicationConstants.MagicUrlStrings.SessionAction)
            {
                logger.LogDebug("Session action detected. Searching for the session Id...");
                var sessionId = context.Request.Headers[ApplicationConstants.MagicUrlStrings.SessionId].ToString().Trim();
                if (!string.IsNullOrEmpty(sessionId))
                {
                    logger.LogDebug("Session Id found - performing looking in the persistence store...");
                    var session = await accountRepository.GetSessionOrNull(sessionId);
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
            Console.WriteLine("On the way out!");
        }
    }


    public class SetAccountEvent : INotification
    {
        public string SessionAccountId { get; }

        public SetAccountEvent(string sessionAccountId)
        {
            SessionAccountId = sessionAccountId;
        }
    }

    public class SetAccountHandler : INotificationHandler<SetAccountEvent>
    {
        private readonly IHoldAnAccountId accountIdHolder;

        public SetAccountHandler(IHoldAnAccountId accountIdHolder)
        {
            this.accountIdHolder = accountIdHolder;
        }

        public async Task Handle(SetAccountEvent notification, CancellationToken cancellationToken)
        {
            accountIdHolder.Assign(notification.SessionAccountId);
            await Task.CompletedTask;
        }
    }
}