using System.Threading.Tasks;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.WidgetLive
{
    public interface ICompileSenderDetails
    {
        Task<CompileSenderDetails.CompiledSenderDetails> Compile(string accountId, string areaId, EmailRequest emailRequest);
    }

    public class CompileSenderDetails : ICompileSenderDetails
    {
        private readonly IAccountRepository accountRepository;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IResponseCustomizer responseCustomizer;

        public CompileSenderDetails(
            IAccountRepository accountRepository,
            IConfigurationRepository configurationRepository,
            IResponseCustomizer responseCustomizer
        )
        {
            this.accountRepository = accountRepository;
            this.configurationRepository = configurationRepository;
            this.responseCustomizer = responseCustomizer;
        }

        public async Task<CompiledSenderDetails> Compile(string accountId, string areaId, EmailRequest emailRequest)
        {
            var account = await accountRepository.GetAccount(accountId);
            var area = await configurationRepository.GetAreaById(accountId, areaId);
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

            htmlBody = responseCustomizer.Customize(htmlBody, emailRequest, account);

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