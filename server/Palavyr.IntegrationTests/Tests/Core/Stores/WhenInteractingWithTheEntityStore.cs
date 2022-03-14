using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Stores;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.IntegrationTests.DataCreators;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Stores
{
    public static class WhenInteractingWithTheEntityStore
    {
        public static readonly string MisMatchedAccount = "Mismatched-Account-Id";

        public class AndCreatingAnEntity : RealDatabaseIntegrationFixture
        {
            [Fact]
            public async Task Success()
            {
                // arrange
                var newEntity = Account.CreateAccount(EmailAddress, Password, AccountId, ApiKey);
                var store = ResolveStore<Account>();

                // act
                var entity = await store.Create(newEntity);

                // assert
                var result = await store.Get(entity.AccountId, s => s.AccountId);
                result.AccountId.ShouldBe(AccountId);
            }

            [Fact]
            public async Task Throws()
            {
                // arrange
                var newEntity = Account.CreateAccount(EmailAddress, Password, MisMatchedAccount, ApiKey);
                var store = ResolveStore<Account>();

                await Should.ThrowAsync<AccountMisMatchException>(async () => await store.Create(newEntity));
            }

            public AndCreatingAnEntity(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
            {
            }
        }

        public class AndCreatingMany : RealDatabaseIntegrationFixture
        {
            [Fact]
            public async Task Success()
            {
                // arrange
                var convoId = A.RandomId();
                var newEntities = Enumerable
                    .Range(0, 10)
                    .Select(
                        x => ConversationHistory.CreateNew(
                            convoId,
                            A.RandomString(),
                            A.RandomString(),
                            A.RandomId(),
                            false,
                            NodeTypeOption.InfoProvide, AccountId));
                var store = ResolveStore<ConversationHistory>();

                // act
                await store.CreateMany(newEntities);

                // assert
                var result = await store.GetMany(convoId, s => s.ConversationId);
                result.Count.ShouldBe(10);
            }

            [Fact]
            public async Task Throws()
            {
                var convoId = A.RandomId();
                var newEntities = Enumerable
                    .Range(0, 10)
                    .Select(
                        x => ConversationHistory.CreateNew(
                            convoId,
                            A.RandomString(),
                            A.RandomString(),
                            A.RandomId(),
                            false,
                            NodeTypeOption.InfoProvide,
                            MisMatchedAccount));
                var store = ResolveStore<ConversationHistory>();


                await Should.ThrowAsync<AccountMisMatchException>(async () => await store.CreateMany(newEntities));
            }

            public AndCreatingMany(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
            {
            }
        }

        public class AndGettingASingleEntity : RealDatabaseIntegrationFixture
        {
            public AndGettingASingleEntity(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
            {
            }

            [Fact]
            public async Task GetsOneEntity()
            {
                var newEntity = Account.CreateAccount(EmailAddress, Password, AccountId, ApiKey);
                var store = ResolveStore<Account>();

                await store.Create(newEntity);

                var result = await store.Get(AccountId, s => s.AccountId);
                result.AccountId.ShouldBe(AccountId);
            }

            [Fact]
            public async Task GetsNull()
            {
                var newEntity = Account.CreateAccount(EmailAddress, Password, AccountId, ApiKey);
                var store = ResolveStore<Account>();

                await store.Create(newEntity);

                var result = await store.GetOrNull(MisMatchedAccount, s => s.AccountId);
                result.ShouldBeNull();
            }

            [Fact]
            public async Task ThrowsEntityNotFound()
            {
                var newEntity = Account.CreateAccount(EmailAddress, Password, MisMatchedAccount, ApiKey);
                var store = ResolveStore<Account>();
                await store.DangerousRawQuery().AddAsync(newEntity);
                var contextProvider = ResolveType<IUnitOfWorkContextProvider>();
                await contextProvider.DangerousCommitAllContexts();

                Should.Throw<EntityNotFoundException>(async () => await store.Get(AccountId, s => s.AccountId));
            }

            [Fact]
            public async Task ThrowsMismatchError()
            {
                var newEntity = Account.CreateAccount(EmailAddress, Password, MisMatchedAccount, ApiKey);
                var store = ResolveStore<Account>();
                var contextProvider = ResolveType<IUnitOfWorkContextProvider>();
                await store.DangerousRawQuery().AddAsync(newEntity);
                await contextProvider.DangerousCommitAllContexts();

                Should.Throw<EntityNotFoundException>(async () => await store.Get(MisMatchedAccount, s => s.AccountId));
            }
        }


        public class AndGettingManyEntities : RealDatabaseIntegrationFixture
        {
            public AndGettingManyEntities(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
            {
            }

            private IEnumerable<StaticTableRow> CreateData(int total, string id)
            {
                return Enumerable
                    .Range(0, 5)
                    .Select(x => StaticTableRow.CreateStaticTableRowTemplate(x, id, AccountId));
            }

            [Fact]
            public async Task ManyAreGottenFromASingleId()
            {
                var store = ResolveStore<StaticTableRow>();

                var intentA = A.RandomId();
                await store.CreateMany(CreateData(5, intentA));

                var intentB = A.RandomId();
                await store.CreateMany(CreateData(5, intentB));

                var results = await store.GetMany(intentA, s => s.AreaIdentifier);

                results.Count.ShouldBe(5);
            }

            [Fact]
            public async Task WithManyAreGottenFromManyIds()
            {
                var store = ResolveStore<StaticTableRow>();

                var intentA = A.RandomId();
                await store.CreateMany(CreateData(5, intentA));

                var intentB = A.RandomId();
                await store.CreateMany(CreateData(5, intentB));

                var intentC = A.RandomId();
                await store.CreateMany(CreateData(5, intentC));

                var ids = new[] { intentA, intentC };
                var results = await store.GetMany(ids, s => s.AreaIdentifier);
                results.Count.ShouldBe(10);
                results.Select(x => x.AreaIdentifier).Distinct().ShouldBe(ids);
            }

            [Fact]
            public async Task AllAreGotten()
            {
                var store = ResolveStore<StaticTableRow>();

                var intentA = A.RandomId();
                await store.CreateMany(CreateData(5, intentA));

                var intentB = A.RandomId();
                await store.CreateMany(CreateData(5, intentB));

                var intentC = A.RandomId();
                await store.CreateMany(CreateData(5, intentC));

                var ids = new[] { intentA, intentB, intentC };
                var results = await store.GetMany(ids, s => s.AreaIdentifier);
                results.Count.ShouldBe(15);
            }


            [Fact]
            public async Task NothingIsReturned()
            {
                var store = ResolveStore<StaticTableRow>();

                var intentId = A.RandomId();
                await store.DangerousRawQuery().AddRangeAsync(
                    Enumerable
                        .Range(0, 5)
                        .Select(x => StaticTableRow.CreateStaticTableRowTemplate(x, intentId, MisMatchedAccount)));
                var contextProvider = ResolveType<IUnitOfWorkContextProvider>();
                await contextProvider.DangerousCommitAllContexts();

                var ids = new[] { intentId };
                var result = await store.GetMany(ids, s => s.AreaIdentifier);
                result.Count.ShouldBe(0);
            }
        }

        public class AndGettingAllEntities : RealDatabaseIntegrationFixture
        {
            public AndGettingAllEntities(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
            {
            }

            [Fact]
            public async Task AllAreGotten()
            {
                var store = ResolveStore<ConversationHistory>();
                var newEntities = Enumerable
                    .Range(0, 5)
                    .Select(
                        x => ConversationHistory.CreateNew(
                            A.RandomId(),
                            A.RandomString(),
                            A.RandomString(),
                            A.RandomId(),
                            false,
                            NodeTypeOption.InfoProvide,
                            AccountId));

                await store.CreateMany(newEntities);

                var results = await store.GetAll();

                results.Length.ShouldBe(5);
            }
        }

        public class AndUpdatingAnEntity : RealDatabaseIntegrationFixture
        {
            public AndUpdatingAnEntity(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
            {
            }

            [Fact]
            public async Task EntityIsUpdated()
            {
                // arrange
                var newEntity = Account.CreateAccount(EmailAddress, Password, AccountId, ApiKey);
                var store = ResolveStore<Account>();

                // act
                var entity = await store.Create(newEntity);
                entity.ApiKey = "woooow";
                await store.Update(entity);

                var result = await store.Get(AccountId, x => x.AccountId);
                result.ApiKey.ShouldBe("woooow");
            }

            [Fact]
            public async Task InvalidOperationExceptionIsThrow()
            {
                var newEntity = Account.CreateAccount(EmailAddress, Password, AccountId, ApiKey);
                var store = ResolveStore<Account>();

                // act
                var entity = await store.Create(newEntity);
                entity.ApiKey = "woooow";
                entity.Id = null;
                await Should.ThrowAsync<InvalidOperationException>(async () => await store.Update(entity));
        }

        public class AndDeletingEntities : RealDatabaseIntegrationFixture
        {
            public AndDeletingEntities(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
            {
            }

            [Fact]
            public async Task SingleEntityIsDeleted()
            {
                throw new NotImplementedException();
            }

            [Fact]
            public async Task SingleEntityIsDeletedById()
            {
                throw new NotImplementedException();
            }

            [Fact]
            public async Task MultipleEntitiesAreDeletedByIds()
            {
                throw new NotImplementedException();
            }

            [Fact]
            public async Task MultipleEntitiesAreDeleted()
            {
                throw new NotImplementedException();
            }
        }
    }
}