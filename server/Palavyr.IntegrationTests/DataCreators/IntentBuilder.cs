using System.Threading.Tasks;
using Palavyr.API.Controllers.Intents;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Test.Common.Random;

namespace Palavyr.IntegrationTests.DataCreators
{
    public static partial class BuilderExtensionMethods
    {
        public static IntentBuilder CreateIntentBuilder(this IntegrationTest test)
        {
            return new IntentBuilder(test);
        }
    }

    public class IntentBuilder
    {
        private readonly IntegrationTest test;
        private bool? sendPdfResponse;
        private string? emailTemplate;
        private string? subject;

        private string? name;

        public IntentBuilder(IntegrationTest test)
        {
            this.test = test;
        }

        public IntentBuilder WithoutResponsePdf()
        {
            this.sendPdfResponse = false;
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

            if (sendPdfResponse != null)
            {
                newIntent.SendPdfResponse = this.sendPdfResponse.Value;
                await test.Client.Post<ModifySendResponsePdfController
            }

            if (emailTemplate != null && !string.IsNullOrEmpty(emailTemplate))
            {
                newIntent.EmailTemplate = emailTemplate;
            }

            if (subject != null && !string.IsNullOrEmpty(subject))
            {
                newIntent.Subject = subject;
            }

            if (name != null && !string.IsNullOrEmpty(name))
            {
                newIntent.AreaName = name;
            }

            
            return newIntent;
        }
    }
}