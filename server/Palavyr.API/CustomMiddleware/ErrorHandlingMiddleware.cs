using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Autofac.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Data;
using Palavyr.Core.Exceptions;
using Stripe;

namespace Palavyr.API.CustomMiddleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> logger;
        private readonly IArrayPool<char> arrayPool = new ArrayPool();

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env, AppDataContexts rawDataContext, IDetermineCurrentEnvironment environment)
        {
            try
            {
                logger.LogDebug("Entering error handling middleware...");
                await next(context);
            }
            catch (Exception ex)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var message = "Bad Request";
                var additionalMessages = new string[] { };

                message = ProcessException(environment, ex, message, ref statusCode, ref additionalMessages);

                if (context.Response.HasStarted)
                {
                    return;
                }

                await FormatErrors(context, message, additionalMessages, statusCode);
            }
        }

        private string ProcessException(IDetermineCurrentEnvironment environment, Exception ex, string message, ref int statusCode, ref string[] additionalMessages)
        {
            switch (ex)
            {
                case StripeException stripeException:
                    logger.LogInformation("Encountered an exception with Stripe");
                    logger.LogError("{Exception}", stripeException.Message);
                    message = ex.Message;
                    break;

                case IOException ioException:
                    logger.LogInformation("File IO exception");
                    logger.LogError("{Exception}", ioException.Message);
                    break;

                case HttpRequestException httpRequestException:
                    logger.LogInformation("Failed to create/write PDF to S3");
                    logger.LogError("{Exception}", httpRequestException.Message);
                    break;

                case AmazonS3Exception amazonS3Exception:
                    logger.LogInformation("Failed to communicate with S3");
                    logger.LogError("{ExceptionMessage}", amazonS3Exception.Message);
                    break;

                case GuidNotFoundException guidNotFoundException:
                    logger.LogInformation("{ExceptionMessage}", "Failed to find a GUID substring.");
                    logger.LogError("{ExceptionMessage}", guidNotFoundException.Message);
                    break;

                case MultiMessageDomainException multiMessageDomainException: // must come before domain exception
                    logger.LogInformation("{ExceptionMessage}", "A domain exception was encountered.");
                    logger.LogError("{ExceptionMessage}", multiMessageDomainException.Message);
                    statusCode = StatusCodes.Status400BadRequest;
                    message = multiMessageDomainException.Message;
                    additionalMessages = multiMessageDomainException.AdditionalMessages;
                    break;

                case DomainException domainException:
                    logger.LogInformation("{ExceptionMessage}", "A domain exception was encountered.");
                    logger.LogError("{ExceptionMessage}", domainException.Message);
                    statusCode = StatusCodes.Status400BadRequest;
                    message = domainException.Message;
                    break;

                case MicroserviceException microserviceException:
                    logger.LogInformation("{ExceptionMessage}", "A Microservice exception was encountered.");
                    logger.LogError("{ExceptionMessage}", microserviceException.Message);
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = microserviceException.Message;
                    break;

                case DependencyResolutionException dependencyResolutionException:
                    logger.LogDebug("{ExceptionMessage}", "A dependency wasn't registered!");
                    logger.LogDebug("{ExceptionMessage}", dependencyResolutionException.Message);
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "Sorry! We made a mistake internally! Please reach out and let us know!";
                    break;

                case AccountMisMatchException accountMisMatchException:
                    logger.LogCritical("{ExceptionMessage}", "An attempt was made to access data across accounts. This is not allowed.");
                    logger.LogCritical("{ExceptionMessage}", accountMisMatchException.Message);
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "Sorry! We made a mistake internally! Please reach out and let us know!";
                    if (environment.IsDevelopment())
                        message = message + $" {accountMisMatchException.Message}";
                    break;

                default:
                    logger.LogError("Unknown Exception");
                    logger.LogError("{ExceptionMessage}", ex.Source);
                    logger.LogError("{ExceptionMessage}", ex.StackTrace);
                    logger.LogError("{ExceptionMessage}", ex.GetType().Name);
                    logger.LogError("{ExceptionMessage}", ex.Message);
                    logger.LogError("{ExceptionMessage}", ex.InnerException);
                    logger.LogError("{ExceptionMessage}", ex.GetBaseException());
                    message = ex.Message;
                    // message = "Oops! We've made a mistake. Please consider reporting this to info.palavyr.com, or you can visit https://github.com/Palavyr/Palavyr-Issues";
                    break;
            }

            return message;
        }

        public async Task FormatErrors(HttpContext context, string message, string[] additionalMessages, int statusCode)
        {
            context.Response.ContentType = "application/json; charset=UTF-8";
            context.Response.StatusCode = statusCode;

            foreach (var additionalMessage in additionalMessages)
            {
                logger.LogError("{ExceptionMessage}", additionalMessage);
            }

            var errorResponse = new ErrorResponse(message, additionalMessages, statusCode);

            // I found it very challenging to write directly to the response body with a structure I can control, so I'm clearing the lot
            // and serializing my own structure. I'll handle this in the client error response handler
            using var writer = new JsonTextWriter(new HttpResponseStreamWriter(context.Response.Body, Encoding.UTF8))
            {
                CloseOutput = false,
                AutoCompleteOnClose = false,
                ArrayPool = arrayPool
            };

            var serializer = JsonSerializer.Create();
            serializer.Serialize(writer, errorResponse);
            await writer.FlushAsync();
        }
    }
}