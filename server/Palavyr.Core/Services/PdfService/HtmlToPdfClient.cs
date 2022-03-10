#nullable enable
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.PdfService.PdfServer;
using Palavyr.Core.Services.TemporaryPaths;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.PdfService
{
    public class HtmlToPdfClient : IHtmlToPdfClient
    {
        private readonly ILogger<HtmlToPdfClient> logger;
        private readonly IPdfServerClient pdfServerClient;
        private readonly ICompilePdfServerRequest compilePdfServerRequest;

        public HtmlToPdfClient(
            ILogger<HtmlToPdfClient> logger,
            IPdfServerClient pdfServerClient,
            ICompilePdfServerRequest compilePdfServerRequest)
        {
            this.logger = logger;
            this.pdfServerClient = pdfServerClient;
            this.compilePdfServerRequest = compilePdfServerRequest;
        }

        public async Task<PdfServerResponse> GeneratePdfFromHtml(string htmlString, string bucket, string locationKey, string identifier, Paper paperOptions)
        {
            var request = compilePdfServerRequest.Compile(bucket, locationKey, htmlString, identifier, paperOptions);
            var pdfServerResponse = await pdfServerClient.PostToPdfServer(request);

            logger.LogDebug($"Successfully wrote the pdf file to disk at {pdfServerResponse.FileAsset.LocationKey}!");
            return pdfServerResponse;
        }
    }

    public class HtmlToPdfClientFileAssetCreatingDecorator : IHtmlToPdfClient
    {
        private readonly IEntityStore<FileAsset> fileAssetStore;
        private readonly IHtmlToPdfClient htmlToPdfClient;
        private readonly IAccountIdTransport accountIdTransport;

        public HtmlToPdfClientFileAssetCreatingDecorator(
            IEntityStore<FileAsset> fileAssetStore,
            IHtmlToPdfClient htmlToPdfClient,
            IAccountIdTransport accountIdTransport)
        {
            this.fileAssetStore = fileAssetStore;
            this.htmlToPdfClient = htmlToPdfClient;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<PdfServerResponse> GeneratePdfFromHtml(string htmlString, string bucket, string locationKey, string identifier, Paper paperOptions)
        {
            var response = await htmlToPdfClient.GeneratePdfFromHtml(htmlString, bucket, locationKey, identifier, paperOptions);
            var newFileAsset = new FileAsset
            {
                AccountId = accountIdTransport.AccountId,
                FileId = identifier,
                LocationKey = response.FileAsset.LocationKey,
                RiskyNameStem = identifier,
                Extension = ExtensionTypes.Pdf
            };

            await fileAssetStore.Create(newFileAsset);
            return response;
        }
    }
}