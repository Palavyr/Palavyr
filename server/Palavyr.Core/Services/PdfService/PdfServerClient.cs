using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Palavyr.Core.Services.PdfService
{
    public interface IPdfServerClient
    {
        Task<PdfServerResponse> PostToPdfServer(FormUrlEncodedContent content);
    }

    public class PdfServerClient : IPdfServerClient
    {
        private readonly HttpClient httpClient = new HttpClient(new HttpClientHandler());
        private readonly ILogger<PdfServerClient> logger;

        public PdfServerClient(ILogger<PdfServerClient> logger)
        {
            this.logger = logger;
        }

        public async Task<PdfServerResponse> PostToPdfServer(FormUrlEncodedContent content)
        {
            try
            {
                var response = await httpClient.PostAsync(PdfServerConstants.PdfServiceUrl, content);
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new HttpRequestException($"Unable to save file to Pdf Server: {response.RequestMessage}");
                }

                var result = JsonConvert.DeserializeObject<PdfServerResponse>(await response.Content.ReadAsStringAsync());

                if (!File.Exists(result.FullPath))
                {
                    logger.LogDebug("PDF File not written correctly");
                    throw new IOException($"Pdf file not written correctly to {result.FullPath}");
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Failed to convert and write the HTML to PDF using the express server.");
                logger.LogCritical($"Attempted to use url: {PdfServerConstants.PdfServiceUrl}");
                logger.LogCritical($"Encountered Error: {ex.Message}");
                throw;
            }
        }
    }
}