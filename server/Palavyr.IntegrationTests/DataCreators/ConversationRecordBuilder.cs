using System;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Stores;
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

        public static async Task CreateAndSave<TEntity>(this BaseIntegrationFixture test, TEntity entity) where TEntity : class, IEntity
        {
            var store = test.ResolveStore<TEntity>();
            store.ResetCancellationToken(new CancellationTokenSource(test.Timeout));

            await store.Create(entity);
            await test.ResolveType<IUnitOfWorkContextProvider>().DangerousCommitAllContexts();
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

            await test.CreateAndSave(record);

            return record;
        }
    }
}