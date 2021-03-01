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

            await next(context);
        }
    }
}