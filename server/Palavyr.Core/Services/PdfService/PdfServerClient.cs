using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Configuration;
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
        private readonly IS3Downloader is3Downloader;
        private readonly ConfigContainerServer config;
        private readonly int retryCount = 60; // number of half seconds
        private readonly string serverUrl = "localhost:5603";

        public PdfServerClient(ILogger<PdfServerClient> logger, IS3Downloader is3Downloader, ConfigContainerServer config)
        {
            this.logger = logger;
            this.is3Downloader = is3Downloader;
            this.config = config;
        }

        private StringContent SerializeRequestObject(PdfServerRequest requestObject)
        {
            var serialized = JsonConvert.SerializeObject(requestObject);
            return new StringContent(serialized, Encoding.UTF8, "application/json");
        }

        public async Task<PdfServerResponse> PostToPdfServer(PdfServerRequest requestObject)
        {
            // var fullPdfServerUrl = configuration.AwsPdfUrl;

            var requestBody = SerializeRequestObject(requestObject);

            logger.LogDebug("Attempting to post to pdf service at {FullPdfServerUrl}", serverUrl);

            HttpResponseMessage response;
            try
            {
                response = await httpClient.PostAsync(serverUrl, requestBody);
            }
            catch (HttpRequestException ex)
            {
                logger.LogCritical("Failed to convert and write the HTML to PDF using the express server");
                logger.LogCritical("Attempted to use url: {pdfServerUri}", serverUrl);
                logger.LogCritical("Encountered Error: {Message}", ex.Message);
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
                found = await is3Downloader.CheckIfFileExists(requestObject.Bucket, requestObject.Key);
                if (count > retryCount)
                {
                    logger.LogDebug("PDF File not written correctly");
                    throw new IOException($"Pdf file not written correctly to {result.S3Key}");
                }

                if (!found)
                {
                    await Task.Delay(500);
                }

                count++;
            }

            return result;
        }
    }
}