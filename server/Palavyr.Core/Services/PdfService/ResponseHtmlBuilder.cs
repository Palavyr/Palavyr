using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.PdfService.PdfSections;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.ResponseCustomization;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.PdfService
{
    public class ResponseHtmlBuilder : IResponseHtmlBuilder
    {
        private readonly IEntityStore<Logo> logoStore;
        private readonly ILinkCreator linkCreator;
        private readonly IConfiguration configuration;
        private readonly IEntityStore<Account> accountStore;
        private readonly IEntityStore<Intent> intentStore;

        public ResponseHtmlBuilder(
            ILinkCreator linkCreator,
            IConfiguration configuration,
            IEntityStore<Logo> logoStore,
            IEntityStore<Account> accountStore,
            IEntityStore<Intent> intentStore)
        {
            this.logoStore = logoStore;
            this.linkCreator = linkCreator;
            this.configuration = configuration;
            this.accountStore = accountStore;
            this.intentStore = intentStore;
        }

        public async Task<string> BuildResponseHtml(
            string intentId,
            CriticalResponses criticalResponses,
            List<Table> pricingStrategyThenStaticTables,
            EmailRequest emailRequest)
        {
            var account = await accountStore.Get(accountStore.AccountId, x => x.AccountId);
            var intent = await intentStore.GetIntentOnly(intentId);
            var previewBuilder = new StringBuilder();

            var logo = await logoStore.GetOrNull(accountStore.AccountId, x => x.AccountId);
            string logoLink = null!;
            if (logo != null)
            {
                if (!string.IsNullOrEmpty(logo.AccountLogoFileId))
                {
                    logoLink = await linkCreator.CreateLink(logo.AccountLogoFileId);
                }
            }

            var options = new ResponseCustomizationOptions
            {
                LogoLink = logoLink ?? "",
                CompanyName = account.CompanyName,
                PhoneNumber = account.PhoneNumber,
                EmailAddress = string.IsNullOrEmpty(intent.IntentSpecificEmail) ? account.EmailAddress : intent.IntentSpecificEmail
            };


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
            previewBuilder.Append(HeaderSection.GetHeader(options, linkCreator, userDataBucket));
            previewBuilder.Append(IntentTitleSection.GetIntentDisplayTitle(intent.IntentName, emailRequest.ConversationId));
            previewBuilder.Append(PrologueSection.GetPrologue(intent.Prologue));
            previewBuilder.Append(TablesSection.GetEstimateTables(pricingStrategyThenStaticTables));
            previewBuilder.Append(EpilogueSection.GetEpilogue(intent.Epilogue));
            previewBuilder.Append(ConversationDetailsSection.GetConversationDetails(emailRequest, criticalResponses));

            previewBuilder.Append(@"</div></body></html>");

            var html = previewBuilder.ToString();

            return html;
        }
    }

    public class ResponseHtmlCustomizationDecorator : IResponseHtmlBuilder
    {
        private readonly IResponseHtmlBuilder builder;
        private readonly IResponseCustomizer responseCustomizer;

        public ResponseHtmlCustomizationDecorator(IResponseHtmlBuilder builder, IResponseCustomizer responseCustomizer)
        {
            this.builder = builder;
            this.responseCustomizer = responseCustomizer;
        }

        public async Task<string> BuildResponseHtml(string intentId, CriticalResponses criticalResponses, List<Table> pricingStrategyThenStaticTables, EmailRequest emailRequest)
        {
            var html = await builder.BuildResponseHtml(intentId, criticalResponses, pricingStrategyThenStaticTables, emailRequest);
            var customizedHtml = await responseCustomizer.Customize(html, emailRequest);
            return customizedHtml;
        }
    }
}