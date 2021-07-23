﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
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
                        statusCode = StatusCodes.Status400BadRequest;
                        message = domainException.Message;
                        break;

                    default:
                        logger.LogError("Unknown Exception");
                        logger.LogError($"{ex.Source}");
                        logger.LogError($"{ex.StackTrace}");
                        logger.LogError($"{ex.GetType().Name}");
                        logger.LogError($"{ex.Message}");
                        logger.LogError($"{ex.InnerException}");
                        logger.LogError($"{ex.GetBaseException()}");
                        break;
                }

                if (context.Response.HasStarted)
                {
                    return;
                }

                await FormatErrors(context, message, statusCode);
            }
        }

        public async Task FormatErrors(HttpContext context, string message, int statusCode)
        {
            // var headers = context.Response.Headers;

            // context.Response.Clear();
            context.Response.ContentType = "application/json; charset=UTF-8";
            context.Response.StatusCode = statusCode;

            var errorResponse = new ErrorResponse(new[] {message}, statusCode).ToString();

            // foreach (var header in headers)
            // {
            //     context.Response.Headers.TryAdd(header.Key, header.Value);
            // }

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
        public string[] Messages { get; set; }
        public int StatusCode { get; set; }

        public ErrorResponse(string[] messages, int statusCode)
        {
            Messages = messages;
            StatusCode = statusCode;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}