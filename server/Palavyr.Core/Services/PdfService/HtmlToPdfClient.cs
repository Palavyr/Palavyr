#nullable enable
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.PdfService.PdfServer;

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

        public async Task<PdfServerResponse> GeneratePdfFromHtml(string htmlString, string bucket, string s3Key, string identifier, Paper paperOptions)
        {
            var request = CompilePdfServerRequest.Compile(bucket, s3Key, htmlString, identifier, paperOptions);
            var pdfServerResponse = await pdfServerClient.PostToPdfServer(request);

            logger.LogDebug($"Successfully wrote the pdf file to disk at {pdfServerResponse.FullPath}!");
            return pdfServerResponse;
        }
    }
}