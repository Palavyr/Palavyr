using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Data;

namespace Palavyr.API.CustomMiddleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env, AccountsContext accountContext)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogDebug("There was a critical exception in the request pipeline, or in the server. Refer to the following message:");
                logger.LogDebug("-------- Error Middleware exception -----------");
                logger.LogDebug(ex.Message);
                logger.LogDebug("-----------------------------------------------");
            }
        }
    }
}