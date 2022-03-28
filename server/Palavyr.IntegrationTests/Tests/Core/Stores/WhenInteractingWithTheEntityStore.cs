using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Stores;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Stores
{
    public class StoreBaseFixture : InMemoryIntegrationFixture
    {
        public StoreBaseFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        public override async Task DisposeAsync()
        {
            await Task.CompletedTask;
            // return base.DisposeAsync();
        }
    }
    
    public static class WhenInteractingWithTheEntityStore
    {
        public static readonly string MisMatchedAccount = "Mismatched-Account-Id";

        public class AndCreatingAnEntity : StoreBaseFixture
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

        public class AndCreatingMany : StoreBaseFixture
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

        public class AndGettingASingleEntity : StoreBaseFixture
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

                await Should.ThrowAsync<EntityNotFoundException>(async () => await store.Get(AccountId, s => s.AccountId));
            }

            [Fact]
            public async Task ThrowsMismatchError()
            {
                var newEntity = Account.CreateAccount(EmailAddress, Password, MisMatchedAccount, ApiKey);
                var store = ResolveStore<Account>();
                var contextProvider = ResolveType<IUnitOfWorkContextProvider>();
                await store.DangerousRawQuery().AddAsync(newEntity);
                await contextProvider.DangerousCommitAllContexts();

                await Should.ThrowAsync<EntityNotFoundException>(async () => await store.Get(MisMatchedAccount, s => s.AccountId));
            }
        }


        public class AndGettingManyEntities : StoreBaseFixture
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

        public class AndGettingAllEntities : StoreBaseFixture
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

        public class AndUpdatingAnEntity : StoreBaseFixture
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
                await Should.ThrowAsync<InvalidOperationException>(async () => await store.Update(newEntity));
            }
        }

        public class AndDeletingEntities : StoreBaseFixture
        {
            public AndDeletingEntities(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
            {
            }

            [Fact]
            public async Task SingleEntityIsDeleted()
            {
                // arrange
                var store = ResolveStore<Area>();
                var entity = await store.Create(Area.CreateNewArea(A.RandomName(), AccountId, EmailAddress, true));

                await store.Delete(entity);

                var result = await store.GetOrNull(entity.AreaIdentifier, x => x.AreaIdentifier);
                result.ShouldBeNull();
            }

            [Fact]
            public async Task AMisMatchErrorIsThrown()
            {
                // arrange
                var store = ResolveStore<Area>();
                var context = ResolveType<IUnitOfWorkContextProvider>();

                var accountA = A.RandomId();
                var newA = Area.CreateNewArea(A.RandomName(), accountA, EmailAddress, true);
                await store.DangerousRawQuery().AddAsync(newA);
                await context.DangerousCommitAllContexts();

                await Should.ThrowAsync<AccountMisMatchException>(async () => await store.Delete(newA));
            }

            [Fact]
            public async Task SingleEntityIsDeletedById()
            {
                // arrange
                var store = ResolveStore<Account>();
                var newEntity = Account.CreateAccount(EmailAddress, Password, AccountId, ApiKey);
                await store.Create(newEntity);

                // act
                await store.Delete(newEntity.AccountId, s => s.AccountId);

                var result = await store.GetOrNull(AccountId, x => x.AccountId);
                result.ShouldBeNull();
            }

            [Fact]
            public async Task MultipleEntitiesAreDeletedByIds()
            {
                // arrange
                var store = ResolveStore<Area>();
                var context = ResolveType<IUnitOfWorkContextProvider>();

                var accountA = A.RandomId();
                var accountB = A.RandomId();
                var newA = Area.CreateNewArea(A.RandomName(), accountA, EmailAddress, true);
                var newB = Area.CreateNewArea(A.RandomName(), accountB, EmailAddress, true);

                await store.DangerousRawQuery().AddAsync(newA);
                await store.DangerousRawQuery().AddAsync(newB);
                await context.DangerousCommitAllContexts();


                // act
                await store.Delete(new[] { accountA, accountB }, s => s.AccountId);

                // Assert
                var result = await store.GetOrNull(accountA, x => x.AccountId);
                result.ShouldBeNull();

                var exists = await store.GetOrNull(accountB, x => x.AccountId);
                exists.ShouldBeNull();
            }

            [Fact]
            public async Task MultipleEntitiesAreDeleted()
            {
                // arrange
                var store = ResolveStore<Area>();

                var newA = Area.CreateNewArea(A.RandomName(), AccountId, EmailAddress, true);
                var newB = Area.CreateNewArea(A.RandomName(), AccountId, EmailAddress, true);

                await store.CreateMany(new[] { newA, newB });

                // act
                await store.Delete(new[] { newA, newB });

                // Assert
                var result = await store.GetOrNull(newA.AreaIdentifier, x => x.AreaIdentifier);
                result.ShouldBeNull();

                var exists = await store.GetOrNull(newB.AreaIdentifier, x => x.AreaIdentifier);
                exists.ShouldBeNull();
            }
        }
    }
}