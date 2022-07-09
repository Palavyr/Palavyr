using System.Threading.Tasks;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources;
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
        private bool? sendPdfResponse;
        private string intentId;
        private string accountId;
        private string emailTemplate;
        private string subject;

        private string? name;

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

        public IntentBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public async Task<IntentResource> Build()
        {

            var nm = name ?? A.RandomName();
            var newIntent = await test.Client.Post<CreateIntentRequest, IntentResource>(new CreateIntentRequest { AreaName = nm }, test.CancellationToken);
            return newIntent;
        }
    }
}