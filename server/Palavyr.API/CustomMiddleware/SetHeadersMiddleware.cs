using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Common.requests;

namespace Palavyr.API.CustomMiddleware
{
    public class SetHeaders
    {

        private readonly RequestDelegate _next;
        private static ILogger<SetHeaders> _logger;

        public SetHeaders(RequestDelegate next, ILogger<SetHeaders> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env, AccountsContext accountContext)
        {
            var action = context.Request.Headers[MagicUrlStrings.Action].ToString();
            
            if ( action == MagicUrlStrings.SessionAction)
            {

                var sessionId = context.Request.Headers[MagicUrlStrings.SessionId].ToString();
                if (!string.IsNullOrWhiteSpace(sessionId))
                {
                    var session = accountContext.Sessions.SingleOrDefault(row => row.SessionId == sessionId);
                    if (session != null)
                    {
                        context.Request.Headers[MagicUrlStrings.AccountId] = session.AccountId;
                    }
                }
            }
            // else if (action == MagicUrlStrings.WidgetAccess) // TODO
            // {
            //
            //     try
            //     {
            //         var url = context.Request.Path;
            //         var uri = new Uri(url);
            //         var query = HttpUtility.ParseQueryString(uri.Query);
            //         var apiKey = query.Get("key");
            //         if (apiKey != null)
            //         {
            //             var account = accountContext.Accounts.SingleOrDefault(row => row.ApiKey == apiKey);
            //             if (account != null)
            //                 context.Request.Headers[MagicUrlStrings.AccountId] = account.AccountId;
            //         }
            //     }
            //     catch (Exception ex)
            //     {
            //
            //     }
            // }

            await _next(context);


            // _logger.LogInformation($"Currently operating in {env.EnvironmentName}");
            // var ACTION = context.Request.Headers[Common.requests.MagicUrlStrings.Action].ToString();

            // if (ACTION == MagicUrlStrings.SessionAction) // needs to be in every request header
            // {
            //     var requestedSession = context.Request.Headers[MagicUrlStrings.SessionId].ToString();
            //     var session = accountContext.Sessions.SingleOrDefault(row => row.SessionId == requestedSession);
            //     if (session == null)
            //     {
            //         await context.Response.WriteAsync("Active session could not be found. Please log in.");
            //         throw new Exception("No Active Session Found.");
            //     }
            //     context.Request.Headers[MagicUrlStrings.AccountId] = session.AccountId;
            // }
        }
    }
}