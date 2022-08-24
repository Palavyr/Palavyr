using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using NSubstitute;
using Palavyr.API;
using Palavyr.Component.Mocks;
using Palavyr.Core;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Test.Common;
using Test.Common.Builders.StripeBuilders;
using Test.Common.Random;
using Test.Common.TestFileAssetServices;
using Xunit;

namespace Palavyr.Component.ComponentTestBase
{
    public abstract class ComponentTest : IClassFixture<ComponentClassFixture>, IAsyncLifetime, IHaveStripeCustomerId
    {
        public bool TransportsEnabled { get; set; } = true;

        public ComponentTest(ComponentClassFixture fixture)
        {
            Fixture = fixture;
        }

        public ComponentClassFixture Fixture { get; set; }
        protected internal virtual bool SaveStoreActionsImmediately => true;
        public IContainer Container { get; set; }
        public CancellationToken CancellationToken { get; set; } = new CancellationTokenSource(TimeSpan.FromMinutes(4)).Token;
        public static string ConfirmationToken { get; set; } = Guid.NewGuid().ToString();
        public string StripeCustomerId { get; set; } = A.RandomId();
        public string AccountId { get; set; } = A.RandomId();

        public DateTime CreatedAt = DateTime.Now;

        public virtual async Task InitializeAsync()
        {
            await Task.Yield();
            Container = Fixture.ConfigureClassFixture(
                builder =>
                {
                    BaseCustomizeContainer(builder);
                    OverrideCustomization(builder);
                });

            if (TransportsEnabled)
            {
                SetAccountIdTransport();
                SetCancellationToken();
            }
        }


        public void SetAccountIdTransport()
        {
            var transport = ResolveType<IAccountIdTransport>();
            if (!transport.IsSet())
            {
                transport.Assign(AccountId);
            }
        }

        public void SetCancellationToken()
        {
            var token = ResolveType<ICancellationTokenTransport>();
            if (!token.IsSet())
            {
                token.Assign(CancellationToken);
            }
        }

        public virtual void OverrideCustomization(ContainerBuilder fixtureUnBuiltContainer)
        {
        }

        private void BaseCustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<IntegrationTestFileSaver>().As<IntegrationTestFileSaver>();
            builder.RegisterType<IntegrationTestFileDelete>().As<IFileAssetDeleter>();

            builder.RegisterType<CreateS3TempFile>().As<ICreateS3TempFile>();
            UseFakeStripeCustomerService(builder);

            if (SaveStoreActionsImmediately)
            {
                builder.RegisterGenericDecorator(typeof(IntegrationTestEntityStoreEagerSavingDecorator<>), typeof(IEntityStore<>));
            }

            builder.RegisterType<MockEmailVerificationService>().As<IEmailVerificationService>();
            builder.RegisterType<MockStripeWebhookAuthService>().As<IStripeWebhookAuthService>();
            builder.RegisterType<MockStripeSubscriptionSetter>().As<IStripeSubscriptionSetter>();
            builder.RegisterType<MockSeSEmail>().As<ISesEmail>().SingleInstance();
        }

        private void UseFakeStripeCustomerService(ContainerBuilder builder)
        {
            builder.Register(
                ctx =>
                {
                    var sub = Substitute.For<IStripeCustomerService>();
                    sub.DeleteStripeTestCustomers(default!);
                    return sub;
                });
        }


        public async Task DisposeAsync()
        {
            await Task.Yield();

            Container?.Dispose();
        }

        public IEntityStore<TEntity> ResolveStore<TEntity>() where TEntity : class, IEntity
        {
            var store = Container.Resolve<IEntityStore<TEntity>>();
            return store;
        }

        public TType ResolveType<TType>()
        {
            return Container.Resolve<TType>();
        }
    }
}