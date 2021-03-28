using System.Collections.Generic;
using System.Text;
using Palavyr.Domain.Accounts.Schemas;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.PdfService.PdfSections;
using Palavyr.Services.PdfService.PdfSections.Util;

namespace Palavyr.Services.PdfService
{
    public class ResponseHtmlBuilder : IResponseHtmlBuilder
    {
        public string BuildResponseHtml(Account account, Area previewData, CriticalResponses response, List<Table> staticTables, List<Table> dynamicTables)
        {
            var previewBuilder = new StringBuilder();

            previewBuilder.Append(@"
                    <!DOCTYPE html>
                    <html lang='en'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <title></title>
                        </head>
                        <body>
                            <div>");
            
            previewBuilder.Append(HeaderSection.GetHeader(account, response));
            previewBuilder.Append(AreaTitleSection.GetAreaDisplayTitle(previewData.AreaDisplayTitle));
            previewBuilder.Append(PrologueSection.GetPrologue(previewData.Prologue));
            previewBuilder.Append(TablesSection.GetEstimateTables(staticTables, dynamicTables));
            previewBuilder.Append(EpilogueSection.GetEpilogue(previewData.Epilogue));

            previewBuilder.Append(@"</div></body></html>");
            
            var html = previewBuilder.ToString();
            return html;
        }
    }
}