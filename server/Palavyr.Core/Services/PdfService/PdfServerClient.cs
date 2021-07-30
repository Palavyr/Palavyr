using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.PdfService.PdfServer;

namespace Palavyr.Core.Services.PdfService
{
    public interface IPdfServerClient
    {
        Task<PdfServerResponse> PostToPdfServer(PdfServerRequest request);
    }

    public class PdfServerClient : IPdfServerClient
    {
        private readonly HttpClient httpClient = new HttpClient(new HttpClientHandler());
        private readonly ILogger<PdfServerClient> logger;
        private readonly IS3Retriever s3Retriever;
        private readonly IConfiguration configuration;
        private readonly int retryCount = 60; // number of half seconds

        public PdfServerClient(ILogger<PdfServerClient> logger, IS3Retriever s3Retriever, IConfiguration configuration)
        {
            this.logger = logger;
            this.s3Retriever = s3Retriever;
            this.configuration = configuration;
        }

        private StringContent SerializeRequestObject(PdfServerRequest requestObject)
        {
            var serialized = JsonConvert.SerializeObject(requestObject);
            return new StringContent(serialized, Encoding.UTF8, "application/json");
        }

        public async Task<PdfServerResponse> PostToPdfServer(PdfServerRequest requestObject)
        {
            var host = configuration.GetPdfServerHost();
            var port = configuration.GetPdfServerPort();
            var requestBody = SerializeRequestObject(requestObject);

            HttpResponseMessage response;
            try
            {
                response = await httpClient.PostAsync(PdfServerConstants.PdfServiceUrl(host, port), requestBody);
            }
            catch (HttpRequestException ex)
            {
                logger.LogCritical($"Failed to convert and write the HTML to PDF using the express server.");
                logger.LogCritical($"Attempted to use url: {PdfServerConstants.PdfServiceUrl}");
                logger.LogCritical($"Encountered Error: {ex.Message}");
                // throw;
                throw new MicroserviceException("The PDF service was unreachable.", ex);
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new MicroserviceException($"Unable to create PDF file: {response.RequestMessage}");
            }

            var result = JsonConvert.DeserializeObject<PdfServerResponse>(await response.Content.ReadAsStringAsync());

            var count = 0;
            var found = false;
            while (!found)
            {
                found = await s3Retriever.CheckIfFileExists(requestObject.Bucket, requestObject.Key);
                if (count > retryCount)
                {
                    logger.LogDebug("PDF File not written correctly");
                    throw new IOException($"Pdf file not written correctly to {result.S3Key}");
                }

                if (!found)
                {
                    Thread.Sleep(500);
                }

                count++;
            }

            return result;
        }
    }
}