using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;

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
                logger.LogDebug("Entering error handling middleware...");
                await next(context);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case IOException ioException:
                        logger.LogInformation("File IO exception.");
                        logger.LogError($"{ioException.Message}");
                        break;

                    case HttpRequestException httpRequestException:
                        logger.LogInformation("Failed to create/write PDF to S3");
                        logger.LogError($"{httpRequestException.Message}");
                        break;

                    default:
                        logger.LogError("Unknown Exception");
                        logger.LogError($"{ex.Message}");
                        break;
                }
            }
        }
    }
}