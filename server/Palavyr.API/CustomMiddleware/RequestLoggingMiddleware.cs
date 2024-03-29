﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace Palavyr.API.CustomMiddleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;
        private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;

        public RequestResponseLoggingMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            this.next = next;
            logger = loggerFactory
                .CreateLogger<RequestResponseLoggingMiddleware>();
            recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            await next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            logger.LogDebug("-----------------------------------------------------");
            logger.LogDebug(
                $"Http Response Information:{Environment.NewLine}" +
                $"Schema:{context.Request.Scheme} " +
                $"Host: {context.Request.Host} " +
                $"Path: {context.Request.Path} " +
                $"QueryString: {context.Request.QueryString} " +
                $"Response Body: {text}");
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            logger.LogDebug("-------------------------------------------");
            logger.LogDebug(
                $"Http Request Information:{Environment.NewLine}" +
                $"Schema:{context.Request.Scheme} " +
                $"Host: {context.Request.Host} " +
                $"Path: {context.Request.Path} " +
                $"QueryString: {context.Request.QueryString} " +
                $"Request Body: {ReadStreamInChunks(requestStream)}");
            context.Request.Body.Position = 0;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(
                    readChunk,
                    0,
                    readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }
    }
}