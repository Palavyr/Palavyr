using System;
using System.Threading.Tasks;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Test.Common.Random;

namespace Palavyr.IntegrationTests.DataCreators
{
    public static partial class BuilderExtensionMethods
    {
        public static ConversationRecordBuilder CreateConversationRecordBuilder(this BaseIntegrationFixture test)
        {
            return new ConversationRecordBuilder(test);
        }
    }


    public class ConversationRecordBuilder
    {
        private readonly BaseIntegrationFixture test;
        private string? conversationId = null!;
        private string? accountId = null!;
        private DateTime? timeStamp = null!;
        private string? intentName = null!;
        private string? intentId = null!;

        public ConversationRecordBuilder(BaseIntegrationFixture test)
        {
            this.test = test;
        }

        public ConversationRecordBuilder WithConversationId(string conversationId)
        {
            this.conversationId = conversationId;
            return this;
        }

        public ConversationRecordBuilder WithAccountId(string accountId)
        {
            this.accountId = accountId;
            return this;
        }

        public ConversationRecordBuilder WithTimeStamp(DateTime timeStamp)
        {
            this.timeStamp = timeStamp;
            return this;
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
        
        public async Task<ConversationRecord> Build()
        {
            var intentId = this.intentId ?? A.RandomId();
            var intentName = this.intentName ?? A.RandomName();
            var timeStamp = this.timeStamp ?? DateTime.Now;
            var accountId = this.accountId ?? test.AccountId;
            var conversationId = this.conversationId ?? A.RandomId();
            
            var record = new ConversationRecord()
            {
                AreaIdentifier = intentId,
                AreaName = intentName,
                TimeStamp = timeStamp,
                AccountId = accountId,
                ConversationId = conversationId
            };

            test.ConvoContext.ConversationRecords.Add(record);
            await test.ConvoContext.SaveChangesAsync();

            return record;
        }
    }
}