using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Services.EmailService.EmailResponse
{
    public interface ICompileSenderDetails
    {
        Task<CompileSenderDetails.CompiledSenderDetails> Compile(string areaId, EmailRequest emailRequest);
    }

    public class CompileSenderDetails : ICompileSenderDetails
    {
        private readonly IConfigurationEntityStore<Account> accountStore;
        private readonly IConfigurationEntityStore<Area> intentStore;
        private readonly IResponseCustomizer responseCustomizer;

        public CompileSenderDetails(
            IConfigurationEntityStore<Area> intentStore,
            IConfigurationEntityStore<Account> accountStore,
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
            var area = await intentStore.Get(intentId, s => s.AreaIdentifier);
            var fromAddress = string.IsNullOrWhiteSpace(area.AreaSpecificEmail) ? account.EmailAddress : area.AreaSpecificEmail;

            var subject = area.UseAreaFallbackEmail ? account.GeneralFallbackSubject : area.Subject;
            var htmlBody = area.UseAreaFallbackEmail ? account.GeneralFallbackEmailTemplate : area.EmailTemplate;

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