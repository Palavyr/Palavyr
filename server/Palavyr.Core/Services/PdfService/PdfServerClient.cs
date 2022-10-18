using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Palavyr.Core.Configuration;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.PdfService.PdfServer;
using Serilog;

namespace Palavyr.Core.Services.PdfService
{
    public interface IPdfServerClient
    {
        Task<PdfServerResponse> PostToPdfServer(PdfServerRequest request);
    }

    public class PdfServerClient : IPdfServerClient 
    {
        private readonly HttpClient httpClient = HttpClientFactory.Create();
        private readonly ILogger logger;
        private readonly IS3Downloader is3Downloader;
        private readonly ConfigContainerServer config;
        private const int retryCount = 60; // number of half seconds

        public PdfServerClient(ILogger logger, IS3Downloader is3Downloader, ConfigContainerServer config)
        {
            this.logger = logger;
            this.is3Downloader = is3Downloader;
            this.config = config;
        }

        private static StringContent SerializeRequestObject(PdfServerRequest requestObject)
        {
            var serialized = JsonSerializer.Serialize(requestObject);
            return new StringContent(serialized, Encoding.UTF8, "application/json");
        }

        public async Task<PdfServerResponse> PostToPdfServer(PdfServerRequest requestObject)
        {
            var requestBody = SerializeRequestObject(requestObject);

            logger.Information("Attempting to post to pdf service at {FullPdfServerUrl}", config.AwsPdfUrl);

            HttpResponseMessage response;
            try
            {
                response = await httpClient.PostAsync(config.AwsPdfUrl, requestBody);
            }
            catch (HttpRequestException ex)
            {
                logger.Error(ex, "Encountered an error writing the PDF using {PdfServerUrl}", config.AwsPdfUrl);
                throw new MicroserviceException("The PDF service was unreachable.", ex);
            }

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new MicroserviceException($"Unable to create PDF file: {content}");
            }

            var result = JsonSerializer.Deserialize<PdfServerResponse>(await response.Content.ReadAsStringAsync());

            var count = 0;
            var found = false;
            while (!found)
            {
                found = await is3Downloader.CheckIfFileExists(requestObject.Bucket, requestObject.Key);
                if (count > retryCount)
                {
                    logger.Debug("PDF File not written correctly");
                    throw new IOException($"Pdf file not written correctly to {result?.S3Key}");
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