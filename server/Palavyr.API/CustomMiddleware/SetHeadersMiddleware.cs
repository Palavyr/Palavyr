using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Common.Requests;
using Palavyr.Data;

namespace Palavyr.API.CustomMiddleware
{
    public class SetHeaders
    {

        private readonly RequestDelegate next;
        private ILogger<SetHeaders> logger;

        public SetHeaders(RequestDelegate next, ILogger<SetHeaders> logger)
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