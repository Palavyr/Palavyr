using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.PdfService.PdfSections;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Models.Resources.Requests;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;

namespace Palavyr.Core.Services.PdfService
{
    public class ResponseHtmlBuilder : IResponseHtmlBuilder
    {
        private readonly ILinkCreator linkCreator;
        private readonly IConfiguration configuration;

        public ResponseHtmlBuilder(ILinkCreator linkCreator, IConfiguration configuration)
        {
            this.linkCreator = linkCreator;
            this.configuration = configuration;
        }

        public string BuildResponseHtml(
            Account account,
            Area previewData,
            CriticalResponses criticalResponses,
            List<Table> staticTables,
            List<Table> dynamicTables,
            EmailRequest emailRequest)
        {
            var previewBuilder = new StringBuilder();

            previewBuilder.Append(
                @"
                    <!DOCTYPE html>
                    <html lang='en' style='width: 100%; height: 100%; margin: 0; padding: 0;'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <title></title>   
                        </head>
                        <body>
                            <div>");
            var userDataBucket = configuration.GetUserDataBucket();
            previewBuilder.Append(HeaderSection.GetHeader(account, linkCreator, userDataBucket, string.IsNullOrEmpty(previewData.AreaSpecificEmail) ? account.EmailAddress : previewData.AreaSpecificEmail));
            previewBuilder.Append(AreaTitleSection.GetAreaDisplayTitle(previewData.AreaDisplayTitle, emailRequest.ConversationId));
            previewBuilder.Append(PrologueSection.GetPrologue(previewData.Prologue));
            previewBuilder.Append(TablesSection.GetEstimateTables(staticTables, dynamicTables));
            previewBuilder.Append(EpilogueSection.GetEpilogue(previewData.Epilogue));
            previewBuilder.Append(ConversationDetailsSection.GetConversationDetails(emailRequest, criticalResponses));

            previewBuilder.Append(@"</div></body></html>");

            var html = previewBuilder.ToString();

            return html;
        }
    }
}