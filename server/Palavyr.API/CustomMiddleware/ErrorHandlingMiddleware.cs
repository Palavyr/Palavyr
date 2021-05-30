using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Exceptions;
using Stripe;

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
                    case StripeException stripeException:
                        logger.LogInformation("Encountered an exception with Stripe");
                        logger.LogError($"{stripeException.Message}");
                        break;

                    case IOException ioException:
                        logger.LogInformation("File IO exception.");
                        logger.LogError($"{ioException.Message}");
                        break;

                    case HttpRequestException httpRequestException:
                        logger.LogInformation("Failed to create/write PDF to S3");
                        logger.LogError($"{httpRequestException.Message}");
                        break;

                    case AmazonS3Exception amazonS3Exception:
                        logger.LogInformation("Failed to communicate with S3");
                        logger.LogError($"{amazonS3Exception.Message}");
                        break;

                    case GuidNotFoundException guidNotFoundException:
                        logger.LogInformation("Failed to find a GUID substring.");
                        logger.LogError($"{guidNotFoundException.Message}");
                        break;

                    case DomainException domainException:
                        logger.LogInformation("A domain exception was encountered.");
                        logger.LogError($"{domainException.Message}");
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