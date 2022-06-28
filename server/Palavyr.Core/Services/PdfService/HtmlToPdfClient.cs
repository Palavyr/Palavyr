
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.PdfService.PdfServer;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.PdfService
{
    public class HtmlToPdfClient : IHtmlToPdfClient
    {
        private readonly ILogger<HtmlToPdfClient> logger;
        private readonly IMapToNew<PdfServerResponse, FileAsset> mapper;
        private readonly IPdfServerClient pdfServerClient;
        private readonly ICompilePdfServerRequest compilePdfServerRequest;

        public HtmlToPdfClient(
            ILogger<HtmlToPdfClient> logger,
            IMapToNew<PdfServerResponse, FileAsset> mapper,
            IPdfServerClient pdfServerClient,
            ICompilePdfServerRequest compilePdfServerRequest)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.pdfServerClient = pdfServerClient;
            this.compilePdfServerRequest = compilePdfServerRequest;
        }

        public async Task<FileAsset> GeneratePdfFromHtml(string htmlString, string locationKey, string identifier, Paper paperOptions)
        {
            var request = compilePdfServerRequest.Compile(locationKey, htmlString, identifier, paperOptions);
            var pdfServerResponse = await pdfServerClient.PostToPdfServer(request);
            var fileAsset = await mapper.Map(pdfServerResponse);
            return fileAsset;
        }
    }

    public class HtmlToPdfClientFileAssetCreatingDecorator : IHtmlToPdfClient
    {
        private readonly IEntityStore<FileAsset> fileAssetStore;
        private readonly IHtmlToPdfClient htmlToPdfClient;

        public HtmlToPdfClientFileAssetCreatingDecorator(
            IEntityStore<FileAsset> fileAssetStore,
            IHtmlToPdfClient htmlToPdfClient)
        {
            this.fileAssetStore = fileAssetStore;
            this.htmlToPdfClient = htmlToPdfClient;
        }

        public async Task<FileAsset> GeneratePdfFromHtml(string htmlString, string locationKey, string identifier, Paper paperOptions)
        {
            var fileAsset = await htmlToPdfClient.GeneratePdfFromHtml(htmlString, locationKey, identifier, paperOptions);
            await fileAssetStore.Create(fileAsset);
            return fileAsset;
        }
    }
}