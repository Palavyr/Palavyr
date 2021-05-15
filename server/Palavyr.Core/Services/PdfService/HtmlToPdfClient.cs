#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Palavyr.Core.Services.PdfService
{
    public class HtmlToPdfClient : IHtmlToPdfClient
    {
        private readonly ILogger<HtmlToPdfClient> logger;
        private readonly IPdfServerClient pdfServerClient;

        public HtmlToPdfClient(ILogger<HtmlToPdfClient> logger, IPdfServerClient pdfServerClient)
        {
            this.logger = logger;
            this.pdfServerClient = pdfServerClient;
        }

        public async Task<PdfServerResponse> GeneratePdfFromHtmlOrNull(string htmlString, string localWriteToPath, string identifier)
        {
            var values = new Dictionary<string, string>
            {
                {PdfServerConstants.html, htmlString.Trim()},
                {PdfServerConstants.path, localWriteToPath},
                {PdfServerConstants.id, identifier} // used as label inside the pdf, not part of the save path
            };

            var content = new FormUrlEncodedContent(values);
            var pdfServerResponse = await pdfServerClient.PostToPdfServer(content);

            logger.LogDebug($"Successfully wrote the pdf file to disk at {pdfServerResponse.FullPath}!");
            return pdfServerResponse;
        }
    }
}