using System.Threading.Tasks;
using IntegrationTests.AppFactory.IntegrationTestFixtures;
using MediatR;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace IntegrationTests.DataCreators
{
    public static class CreateDefaultTestAccountBuilderExtensionMethods
    {
        public static DefaultTestAccountBuilder CreateDefaultTestAccountBuilder(this IntegrationTest testBase)
        {
            return new DefaultTestAccountBuilder(testBase);
        }
    }

    public class DefaultTestAccountBuilder
    {
        private readonly IntegrationTest testBase;


        public DefaultTestAccountBuilder(IntegrationTest testBase)
        {
            this.testBase = testBase;
        }

        public async Task<CredentialsResource> Build(string email, string password)
        {
            var credentials = await testBase.Client.Post<CreateNewAccountRequest, CredentialsResource>(
                new CreateNewAccountRequest
                {
                    EmailAddress = email,
                    Password = password
                }, testBase.CancellationToken);

            testBase.SessionId = credentials.SessionId;
            testBase.ApiKey = credentials.ApiKey;

            // TODO: If we make the client lazy again, does it break everything? bc if not, then we can just do this
            // testBase.Client.AddHeader(ApplicationConstants.MagicUrlStrings.SessionId, credentials.SessionId);

            // activate the account
            await testBase.Client.Post<ConfirmEmailAddressRequest, bool>(
                testBase.CancellationToken,
                r => ConfirmEmailAddressRequest.FormatRoute(IntegrationTest.ConfirmationToken));

            // with the mocks registered, we update the account to Pro
            // the upgrade path is deliberately hard - it only happens via the stripe webhooks
            await testBase.Client.Post<ProcessStripeNotificationWebhookRequest, Unit>(testBase.CancellationToken);


            return credentials;
        }
    }
}