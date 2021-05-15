using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.DynamicTableService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PdfService
{
    public class PdfResponseGenerator : IPdfResponseGenerator
    {
        private readonly IAccountRepository accountRepository;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IHtmlToPdfClient htmlToPdfClient;
        private readonly IResponseHtmlBuilder responseHtmlBuilder;
        private readonly IResponseCustomizer responseCustomizer;
        private readonly IStaticTableCompiler staticTableCompiler;
        private readonly IDynamicTableCompilerOrchestrator dynamicTablesCompiler;


        public PdfResponseGenerator(
            IAccountRepository accountRepository,
            IConfigurationRepository configurationRepository,
            IHtmlToPdfClient htmlToPdfClient,
            IResponseHtmlBuilder responseHtmlBuilder,
            IResponseCustomizer responseCustomizer,
            IStaticTableCompiler staticTableCompiler,
            IDynamicTableCompilerOrchestrator dynamicTablesCompiler
        )
        {
            this.accountRepository = accountRepository;
            this.configurationRepository = configurationRepository;
            this.htmlToPdfClient = htmlToPdfClient;
            this.responseHtmlBuilder = responseHtmlBuilder;
            this.responseCustomizer = responseCustomizer;
            this.staticTableCompiler = staticTableCompiler;
            this.dynamicTablesCompiler = dynamicTablesCompiler;
        }

        public async Task<PdfServerResponse> GeneratePdfResponseAsync(
            CriticalResponses criticalResponses,
            EmailRequest emailRequest,
            CultureInfo culture,
            string localWriteToPath,
            string identifier,
            string accountId,
            string areaId,
            CancellationToken cancellationToken
        )
        {
            if (emailRequest.NumIndividuals <= 0) throw new Exception("Num individuals must be 1 or more.");

            var areaData = await configurationRepository.GetAreaComplete(accountId, areaId);
            var account = await accountRepository.GetAccount(accountId, cancellationToken);

            var staticTables = staticTableCompiler.CollectStaticTables(areaData, culture, emailRequest.NumIndividuals); // ui always sends a number - 1 or greater.
            var dynamicTables = await dynamicTablesCompiler.CompileTablesToPdfRows(accountId, emailRequest.DynamicResponses, culture);

            var html = responseHtmlBuilder.BuildResponseHtml(account, areaData, criticalResponses, staticTables, dynamicTables);

            html = responseCustomizer.Customize(html, emailRequest, account);

            var pdfServerResponse = await htmlToPdfClient.GeneratePdfFromHtmlOrNull(html, localWriteToPath, identifier);
            return pdfServerResponse;
        }
    }
}