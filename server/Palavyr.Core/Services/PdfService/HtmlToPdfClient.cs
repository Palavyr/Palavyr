#nullable enable
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Services.PdfService.PdfServer;

namespace Palavyr.Core.Services.PdfService
{
    public class HtmlToPdfClient : IHtmlToPdfClient
    {
        private readonly ILogger<HtmlToPdfClient> logger;
        private readonly IPdfServerClient pdfServerClient;
        private readonly IConfiguration configuration;

        public HtmlToPdfClient(ILogger<HtmlToPdfClient> logger, IPdfServerClient pdfServerClient, IConfiguration configuration)
        {
            this.logger = logger;
            this.pdfServerClient = pdfServerClient;
            this.configuration = configuration;
        }

        public async Task<PdfServerResponse> GeneratePdfFromHtml(string htmlString, string bucket, string s3Key, string identifier, Paper paperOptions)
        {
            var accessKey = configuration.GetAccessKey();
            var secretKey = configuration.GetSecretKey();
            var region = configuration.GetRegion();
            var request = CompilePdfServerRequest.Compile(bucket, s3Key, accessKey, secretKey, region, htmlString, identifier, paperOptions);
            var pdfServerResponse = await pdfServerClient.PostToPdfServer(request);

            logger.LogDebug($"Successfully wrote the pdf file to disk at {pdfServerResponse.S3Key}!");
            return pdfServerResponse;
        }
    }
}