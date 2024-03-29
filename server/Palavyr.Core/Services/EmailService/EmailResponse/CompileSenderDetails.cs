﻿using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Requests;
using Palavyr.Core.Services.ResponseCustomization;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.EmailService.EmailResponse
{
    public interface ICompileSenderDetails
    {
        Task<CompileSenderDetails.CompiledSenderDetails> Compile(string intentId, EmailRequest emailRequest);
    }

    public class CompileSenderDetails : ICompileSenderDetails
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly IEntityStore<Intent> intentStore;
        private readonly IResponseCustomizer responseCustomizer;

        public CompileSenderDetails(
            IEntityStore<Intent> intentStore,
            IEntityStore<Account> accountStore,
            IResponseCustomizer responseCustomizer
        )
        {
            this.accountStore = accountStore;
            this.intentStore = intentStore;
            this.accountStore = accountStore;
            this.responseCustomizer = responseCustomizer;
        }

        public async Task<CompiledSenderDetails> Compile(string intentId, EmailRequest emailRequest)
        {
            var account = await accountStore.GetAccount();
            var intent = await intentStore.Get(intentId, s => s.IntentId);
            var fromAddress = string.IsNullOrWhiteSpace(intent.IntentSpecificEmail) ? account.EmailAddress : intent.IntentSpecificEmail;

            var subject = intent.UseIntentFallbackEmail ? account.GeneralFallbackSubject : intent.Subject;
            var htmlBody = intent.UseIntentFallbackEmail ? account.GeneralFallbackEmailTemplate : intent.EmailTemplate;

            var textBody = htmlBody; // This can be another upload. People can decide one or both. Html is prioritized.
            if (string.IsNullOrWhiteSpace(htmlBody))
            {
                htmlBody = "";
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                subject = "";
            }

            htmlBody = await responseCustomizer.Customize(htmlBody, emailRequest);

            return new CompiledSenderDetails
            {
                FromAddress = fromAddress,
                ToAddress = emailRequest.EmailAddress,
                Subject = subject,
                BodyAsHtml = htmlBody,
                BodyAsText = textBody
            };
        }

        public class CompiledSenderDetails
        {
            public string FromAddress { get; set; }
            public string ToAddress { get; set; }
            public string Subject { get; set; }
            public string BodyAsHtml { get; set; }
            public string BodyAsText { get; set; }
        }
    }
}