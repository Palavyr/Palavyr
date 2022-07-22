using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models;
using Palavyr.Core.Resources;
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
        private string? name;
        private string? intentId;
        private string? email;
        private bool isDemo = false;

        public ConversationRecordBuilder(IntegrationTest test)
        {
            this.test = test;
        }


        public ConversationRecordBuilder WithName(string intentName)
        {
            this.name = intentName;
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
            var name = this.name ?? A.RandomName();
            var email = this.email ?? A.RandomTestEmail();
            var isDemo = this.isDemo;

            var intents = await test.Client.GetResource<GetAllIntentsRequest, IEnumerable<IntentResource>>(test.CancellationToken);
            var intent = intents.SingleOrDefault(x => x.IntentId == id);
            if (intent is null)
            {
                intent = await test.CreateIntentBuilder().Build();
                id = intent.IntentId;
            }

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