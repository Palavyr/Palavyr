using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Palavyr.API;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.Delete;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Test.Common;
using Test.Common.TestFileAssetServices;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture
{
    public abstract class BaseIntegrationFixture : IClassFixture<IntegrationTestAutofacWebApplicationFactory>, IAsyncLifetime
    {
        public readonly string AccountId = Guid.NewGuid().ToString();
        public readonly string ApiKey = Guid.NewGuid().ToString();
        public readonly string StripeCustomerId = Guid.NewGuid().ToString();
        public readonly string SessionId = Guid.NewGuid().ToString();

        public readonly Lazy<AutofacServiceProvider> ServiceProvider;


        public ITestOutputHelper TestOutputHelper { get; set; }
        public readonly IntegrationTestAutofacWebApplicationFactory Factory;

        public WebApplicationFactory<Startup> WebHostFactory { get; set; } = null!;

        public HttpClient Client => WebHostFactory.ConfigureInMemoryClient(SessionId);
        public HttpClient ClientApiKey => WebHostFactory.ConfigureInMemoryApiKeyClient(ApiKey);

        public CancellationToken CancellationToken => new CancellationTokenSource(Timeout).Token;
        public TimeSpan Timeout => TimeSpan.FromMinutes(3);

        public ILogger Logger => WebHostFactory.Services.GetService<ILogger>();

        public IEntityStore<TEntity> ResolveStore<TEntity>() where TEntity : class, IEntity
        {
            var store = WebHostFactory.Services.GetService<IEntityStore<TEntity>>();
            return store;
        }

        public TType ResolveType<TType>()
        {
            return WebHostFactory.Services.GetService<TType>();
        }

        public AutofacServiceProvider Container => ServiceProvider.Value;
        public IConfiguration Configuration => TestConfiguration.GetTestConfiguration();

        protected BaseIntegrationFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory)
        {
            TestOutputHelper = testOutputHelper;
            Factory = factory;

            ServiceProvider = new Lazy<AutofacServiceProvider>(
                () => { return (AutofacServiceProvider)WebHostFactory.Services; });
        }

        public virtual ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<IntegrationTestFileSaver>().As<IntegrationTestFileSaver>();
            builder.RegisterType<IntegrationTestFileDelete>().As<IFileAssetDeleter>();


            builder.RegisterType<CreateS3TempFile>().As<ICreateS3TempFile>();
            UseFakeStripeCustomerService(builder);

            builder.RegisterGenericDecorator(typeof(IntegrationTestEntityStoreEagerSavingDecorator<>), typeof(IEntityStore<>));
            builder.RegisterGenericDecorator(typeof(IntegrationTestMediatorNotificationHandlerDecorator<>), typeof(INotificationHandler<>));
            builder.RegisterGenericDecorator(typeof(IntegrationTestMediatorRequestHandlerDecorator<,>), typeof(IRequestHandler<,>));

            return builder;
        }

        public void UseFakeStripeCustomerService(ContainerBuilder builder)
        {
            builder.Register(
                ctx =>
                {
                    var sub = Substitute.For<IStripeCustomerService>();
                    sub.DeleteStripeTestCustomers(default);
                    return sub;
                });
        }


        private protected virtual async Task DeleteTestStripeCustomers()
        {
            // var customerService = ResolveType<IStripeCustomerService>();
            // await customerService.DeleteSingleStripeTestCustomer(StripeCustomerId);
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
            var token = Container.GetService<ICancellationTokenTransport>();
            if (!token.IsSet())
            {
                token.Assign(CancellationToken);
            }
        }

        public virtual async Task InitializeAsync()
        {
            SetAccountIdTransport();
            await Task.CompletedTask;
        }

        public virtual async Task DisposeAsync()
        {
            // await DeleteTestStripeCustomers();
            SetCancellationToken();
            var deleter = ResolveType<IDangerousAccountDeleter>();
            await deleter.DeleteAllThings();

            var sessionStore = ResolveStore<Session>();
            await sessionStore.Delete(SessionId, s => s.SessionId);

            var provider = ResolveType<IUnitOfWorkContextProvider>();
            await provider.DisposeContexts();

            WebHostFactory.Dispose();
        }
    }

    public class IntegrationTestMediatorRequestHandlerDecorator<TEvent, TResponse> : IRequestHandler<TEvent, TResponse> where TEvent : IRequest<TResponse>
    {
        private readonly IRequestHandler<TEvent, TResponse> inner;
        private readonly IUnitOfWorkContextProvider unitOfWorkContextProvider;

        public IntegrationTestMediatorRequestHandlerDecorator(IRequestHandler<TEvent, TResponse> inner, IUnitOfWorkContextProvider unitOfWorkContextProvider)
        {
            this.inner = inner;
            this.unitOfWorkContextProvider = unitOfWorkContextProvider;
        }

        public async Task<TResponse> Handle(TEvent request, CancellationToken cancellationToken)
        {
            var response = await inner.Handle(request, cancellationToken);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
            return response;
        }
    }

    public class IntegrationTestMediatorNotificationHandlerDecorator<TEvent> : INotificationHandler<TEvent> where TEvent : INotification
    {
        private readonly INotificationHandler<TEvent> inner;
        private readonly IUnitOfWorkContextProvider unitOfWorkContextProvider;

        public IntegrationTestMediatorNotificationHandlerDecorator(INotificationHandler<TEvent> inner, IUnitOfWorkContextProvider unitOfWorkContextProvider)
        {
            this.inner = inner;
            this.unitOfWorkContextProvider = unitOfWorkContextProvider;
        }

        public async Task Handle(TEvent notification, CancellationToken cancellationToken)
        {
            await inner.Handle(notification, cancellationToken);
            await unitOfWorkContextProvider.DangerousCommitAllContexts();
        }
    }
}