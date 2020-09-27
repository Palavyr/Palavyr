using System;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Palavyr.Common.requests;


//https://thecodebuzz.com/access-database-middleware-entity-framework-net-core/
namespace Palavyr.API.CustomMiddleware
{
    public class AuthenticateByLoginOrSession
    {

        private readonly RequestDelegate _next;
        private static ILogger<AuthenticateByLoginOrSession> _logger;

        public AuthenticateByLoginOrSession(RequestDelegate next, ILogger<AuthenticateByLoginOrSession> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env, AccountsContext accountContext)
        {
    
            var ACTION = context.Request.Headers[Common.requests.MagicUrlStrings.Action].ToString();

            if (string.IsNullOrEmpty(ACTION) && (context.Request.Path.ToString().EndsWith("/action/setup")))
            {
                await _next(context);
            }
            
            else if (ACTION == MagicUrlStrings.SessionAction) // needs to be in every request header
            {
                var requestedSession = context.Request.Headers[MagicUrlStrings.SessionId].ToString();
                var session = accountContext.Sessions.SingleOrDefault(row => row.SessionId == requestedSession);
                if (session == null)
                {
                    await context.Response.WriteAsync("Active session could not be found. Please log in.");
                    throw new Exception("No Active Session Found.");
                }
                context.Request.Headers[MagicUrlStrings.AccountId] = session.AccountId;
                await _next(context);
            }
            else if (ACTION == MagicUrlStrings.LoginAction)
            {

                if (context.Request.Path == "/api/authentication/login")
                {
                    await _next(context);
                }
                else if (context.Request.Path == "/api/authentication/session")
                {
                    await _next(context);
                }
                else if (context.Request.Path == "/api/account/create")
                {
                    await _next(context);
                }
                else
                {
                    throw new Exception("Wrong url for login");
                }
            }

            else if (ACTION == MagicUrlStrings.LogoutAction)
            {
                
                if (context.Request.Path == "/api/authentication/logout")
                {
                    var sessionId = context.Request.Headers[MagicUrlStrings.SessionId].ToString();

                    if (sessionId != null)
                    {
                        var session = accountContext.Sessions.SingleOrDefault(row => row.SessionId == sessionId);
                        accountContext.Sessions.Remove(session);
                        await accountContext.SaveChangesAsync();
                    }
                }
                else
                    throw new Exception("Wrong url for logout");
                
            }
            
            else if (ACTION == MagicUrlStrings.WidgetAccess)
            {
                await _next(context);
            }
            
            else if (ACTION == MagicUrlStrings.DevAccess)
            {
                _logger.LogInformation("ACCESSING USING THE DEV BACKDOOR");
                if (env.IsDevelopment())
                {
                    context.Request.Headers[MagicUrlStrings.AccountId] = MagicUrlStrings.DevAccount;
                    await _next(context);
                }
            }

            else
            {
                _logger.LogCritical("NO ACTION HEADER FOUND");
                throw new AccessViolationException("Must provide action string in header.");
                
            }
                
            
        }
    }
}