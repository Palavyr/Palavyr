using System.Threading.Tasks;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Test.Common.Random;

namespace Palavyr.IntegrationTests.DataCreators
{
    public static partial class BuilderExtensionMethods
    {
        public static ConversationRecordBuilder CreateConversationRecordBuilder(this IntegrationTest test)
        {
            return new ConversationRecordBuilder(test);
        }
    }


    public class ConversationRecordBuilder
    {
        private readonly IntegrationTest test;
        private string? intentName;
        private string? intentId;
        private string? email;
        private bool isDemo = false;

        public ConversationRecordBuilder(IntegrationTest test)
        {
            this.test = test;
        }


        public ConversationRecordBuilder WithIntentName(string intentName)
        {
            this.intentName = intentName;
            return this;
        }

        public ConversationRecordBuilder WithIntentId(string intentId)
        {
            this.intentId = intentId;
            return this;
        }

        public ConversationRecordBuilder WithEmail(string email)
        {
            this.email = email;
            return this;
        }

        public ConversationRecordBuilder AsDemo()
        {
            this.isDemo = true;
            return this;
        }

        public async Task<NewConversationResource> Build()
        {
            var id = this.intentId ?? A.RandomId();
            var name = this.intentName ?? A.RandomName();
            var email = this.email ?? A.RandomTestEmail();
            var isDemo = this.isDemo;
            
            var newConversationRecordRequest = new CreateNewConversationHistoryRequest
            {
                IntentId = id,
                Name = name,
                Email = email,
                IsDemo = isDemo
            };

            var newConversationResource = await test.ApikeyClient.Post<CreateNewConversationHistoryRequest, NewConversationResource>(newConversationRecordRequest, test.CancellationToken);
            return newConversationResource;
        }
    }
}