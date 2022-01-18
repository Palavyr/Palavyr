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

        public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env, AccountsContext accountContext)
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

                switch (ex)
                {
                    case StripeException stripeException:
                        logger.LogInformation("Encountered an exception with Stripe");
                        logger.LogError($"{stripeException.Message}");
                        message = ex.Message;
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

                    case MultiMessageDomainException multiMessageDomainException: // must come before domain exception
                        logger.LogInformation("A domain exception was encountered.");
                        logger.LogError($"{multiMessageDomainException.Message}");
                        statusCode = StatusCodes.Status400BadRequest;
                        message = multiMessageDomainException.Message;
                        additionalMessages = multiMessageDomainException.AdditionalMessages;
                        break;

                    case DomainException domainException:
                        logger.LogInformation("A domain exception was encountered.");
                        logger.LogError($"{domainException.Message}");
                        statusCode = StatusCodes.Status400BadRequest;
                        message = domainException.Message;
                        break;

                    case MicroserviceException microserviceException:
                        logger.LogInformation("A Microservice exception was encountered.");
                        logger.LogError($"{microserviceException.Message}");
                        statusCode = StatusCodes.Status500InternalServerError;
                        message = microserviceException.Message;
                        break;
                    
                    case DependencyResolutionException dependencyResolutionException:
                        logger.LogDebug("A dependency wasn't registered!");
                        logger.LogDebug(dependencyResolutionException.Message);
                        statusCode = StatusCodes.Status500InternalServerError;
                        message = "Sorry! We made a mistake internally! Please reach out and let us know!";
                        break;
                        
                    default:
                        logger.LogError("Unknown Exception");
                        logger.LogError($"{ex.Source}");
                        logger.LogError($"{ex.StackTrace}");
                        logger.LogError($"{ex.GetType().Name}");
                        logger.LogError($"{ex.Message}");
                        logger.LogError($"{ex.InnerException}");
                        logger.LogError($"{ex.GetBaseException()}");
                        message = "Oops! We've made a mistake. Please consider reporting this to info.palavyr.com, or you can visit https://github.com/Palavyr/Palavyr-Issues";
                        break;
                }

                if (context.Response.HasStarted)
                {
                    return;
                }

                await FormatErrors(context, message, additionalMessages, statusCode);
            }
        }

        public async Task FormatErrors(HttpContext context, string message, string[] additionalMessage, int statusCode)
        {
            context.Response.ContentType = "application/json; charset=UTF-8";
            context.Response.StatusCode = statusCode;

            var errorResponse = new ErrorResponse(message, additionalMessage, statusCode).ToString();

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

    public class ErrorResponse
    {
        public string Message { get; set; }
        public string[] AdditionalMessages { get; set; }
        public int StatusCode { get; set; }

        public ErrorResponse(string messages, string[] additionalMessages, int statusCode)
        {
            Message = messages;
            AdditionalMessages = additionalMessages;
            StatusCode = statusCode;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}