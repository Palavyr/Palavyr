#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.FileSystemTools.LocalServices;
using File = System.IO.File;

namespace Palavyr.Core.Services.PdfService
{
    public class HtmlToPdfClient : IHtmlToPdfClient
    {
        private readonly ILogger<HtmlToPdfClient> logger;
        private static readonly HttpClient Client = new HttpClient();

        public HtmlToPdfClient(ILogger<HtmlToPdfClient> logger)
        {
            this.logger = logger;
        }

        public async Task<string?> GeneratePdfFromHtmlOrNull(string htmlString, string localWriteToPath, string identifier)
        {
            var values = new Dictionary<string, string>
            {
                {LocalServices.html, htmlString.Trim()},
                {LocalServices.path, localWriteToPath},
                {LocalServices.id, identifier} // used as label inside the pdf, not part of the save path
            };

            var content = new FormUrlEncodedContent(values);

            string localTempOutputPath;
            try
            {
                var response = await Client.PostAsync(LocalServices.PdfServiceUrl, content);
                localTempOutputPath = await response.Content.ReadAsStringAsync();
                logger.LogDebug("Successfully created a PDF file.");
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Failed to convert and write the HTML to PDF using the express server.");
                logger.LogCritical($"Attempted to use url: {LocalServices.PdfServiceUrl}");
                logger.LogCritical($"Encountered Error: {ex.Message}");
                return null;
            }

            if (!File.Exists(localTempOutputPath))
            {
                throw new Exception("PDF File not written correctly");
            }

            logger.LogDebug($"Successfully wrote the pdf file to disk at {localTempOutputPath}!");
            return localTempOutputPath;
        }
    }
}