using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Test.Common.Random;

namespace Palavyr.IntegrationTests.DataCreators
{
    public static partial class BuilderExtensionMethods
    {
        public static IntentBuilder CreateIntentBuilder(this BaseIntegrationFixture test)
        {
            return new IntentBuilder(test);
        }
    }

    public class IntentBuilder
    {
        private readonly BaseIntegrationFixture test;
        private bool? sendPdfResponse = null!;
        private string intentId;
        private string accountId;
        private string emailTemplate;
        private string subject;

        public IntentBuilder(BaseIntegrationFixture test)
        {
            this.test = test;
        }

        public IntentBuilder WithoutResponsePdf()
        {
            this.sendPdfResponse = false;
            return this;
        }

        public IntentBuilder WithIntentId(string intentId)
        {
            this.intentId = intentId;
            return this;
        }

        public IntentBuilder WithAccountId(string accountId)
        {
            this.accountId = accountId;
            return this;
        }

        public IntentBuilder WithEmailTemplate(string emailTemplateString)
        {
            this.emailTemplate = emailTemplateString;
            return this;
        }

        public IntentBuilder WithEmailSubject(string subject)
        {
            this.subject = subject;
            return this;
        }

        public async Task<Area> Build()
        {
            var intentId = this.intentId ?? A.RandomId();
            var sendPdf = this.sendPdfResponse ?? false;
            var accountId = this.accountId ?? test.AccountId;
            var emailTemplate = this.emailTemplate ?? A.RandomString();
            var subject = this.subject ?? A.RandomString();

            var intent = new Area
            {
                AreaIdentifier = intentId,
                SendPdfResponse = sendPdf,
                AccountId = accountId,
                EmailTemplate = emailTemplate,
                Subject = subject
            };

            test.DashContext.Areas.Add(intent);
            await test.DashContext.SaveChangesAsync();
            return intent;
        }
    }
}