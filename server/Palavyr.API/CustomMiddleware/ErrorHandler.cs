using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;
using Stripe;

namespace Palavyr.API.CustomMiddleware
{
    public class ErrorHandler
    {
        public ErrorHandler()
        {
        }

        public async Task HandleErrors(HttpContext context, ILogger logger)
        {
            var error = context.Features.Get<IExceptionHandlerPathFeature>();
            if (error != null)
            {
                var ex = error.Error;
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
                        logger.LogError($"{ex.Source}");
                        logger.LogError($"{ex.StackTrace}");
                        logger.LogError($"{ex.GetType().Name}");
                        logger.LogError($"{ex.Message}");
                        logger.LogError($"{ex.InnerException}");
                        logger.LogError($"{ex.GetBaseException()}");
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                if (context.Response.HasStarted)
                {
                    return;
                }

                await FormatErrors(context, ex.Message, StatusCodes.Status400BadRequest);
            }
        }


        public async Task FormatErrors(HttpContext context, string message, int statusCode)
        {
            // var headers = context.Response.Headers;
            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json; charset=UTF-8";
            // var serialized = new ErrorResponse(new []{message}, statusCode).ToString();
            
            // foreach (var header in headers)
            // {
            //     context.Response.Headers.TryAdd(header.Key, header.Value);
            // }

            // await context.Response.WriteAsync(serialized);
        }
    }
}