using System.Threading.Tasks;
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
            var newIntent = await test.Client.Post<CreateIntentRequest, IntentResource>(new CreateIntentRequest { IntentName = nm }, test.CancellationToken);

            if (sendPdfResponse != null)
            {
                newIntent.SendPdfResponse = this.sendPdfResponse.Value;
                await test.Client.Post<ModifySendResponseRequest, bool>(test.CancellationToken, s => ModifySendResponseRequest.FormatRoute(newIntent.IntentId));
            }

            if (emailTemplate != null && !string.IsNullOrEmpty(emailTemplate))
            {
                newIntent.EmailTemplate = emailTemplate;
                await test.Client.Post<ModifyIntentEmailTemplateRequest, string>(test.CancellationToken);
            }

            if (subject != null && !string.IsNullOrEmpty(subject))
            {
                newIntent.Subject = subject;
                await test.Client.Post<ModifyIntentEmailSubjectRequest, string>(test.CancellationToken);
            }

            if (name != null && !string.IsNullOrEmpty(name))
            {
                newIntent.IntentName = name;
                await test.Client.Post<ModifyIntentNameRequest, string>(test.CancellationToken, _ => ModifyIntentNameRequest.FormatRoute(newIntent.IntentId));
            }

            
            return newIntent;
        }
    }
}