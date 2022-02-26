#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Palavyr.Core.Services.StripeServices.Products;
using Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.InvoiceCreated;
using Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.InvoicePaid;
using Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.PaymentFailed;
using Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.SubscriptionCreated;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Shouldly;
using Stripe;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Services.StripeServices
{
    public class StripeWebhookRoutingServiceFixture : RealDatabaseIntegrationFixture
    {
        private IStripeEventWebhookRoutingService router = null!;
        private ILogger<IStripeSubscriptionService> logger = null!;
        private StagingProductRegistry registry = null!;

        public StripeWebhookRoutingServiceFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }


        [Fact]
        public async Task EventReceivedIsFired()
        {
            var signature = A.RandomId();
            var @event = await WriteAMockEvent(A.RandomName(), signature);

            await router.ProcessStripeEvent(@event, signature, CancellationToken.None);

            var mediator = (IGetData)Container.GetService<IMediator>();
            mediator.Get()[0].Name.ShouldBe(nameof(NewStripeEventReceivedEvent));
        }

        [Fact]
        public async Task SkipsEvent()
        {
            var signature = A.RandomId();
            var @event = await WriteAMockEvent(A.RandomName(), signature);

            await router.ProcessStripeEvent(@event, signature, CancellationToken.None);

            var mediator = (IGetData)Container.GetService<IMediator>();
            mediator.GetFiltered().Count.ShouldBe(0);
        }


        [Fact]
        public async Task CustomerSubscriptionUpdatedRoute()
        {
            var @event = CreateAMockEvent(Events.CustomerSubscriptionUpdated);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            AssertEventIsRouted<SubscriptionUpdatedEvent>();
        }

        [Fact]
        public async Task CustomerSubscriptionCreated()
        {
            var @event = CreateAMockEvent(Events.CustomerSubscriptionCreated);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            AssertEventIsRouted<SubscriptionCreatedEvent>();
        }


        [Fact]
        public async Task CustomerSubscriptionDeleted()
        {
            var @event = CreateAMockEvent(Events.CustomerSubscriptionDeleted);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            AssertEventIsRouted<SubscriptionDeletedEvent>();
        }

        [Fact]
        public async Task CheckoutSessionCompleted()
        {
            var @event = CreateAMockEvent(Events.CheckoutSessionCompleted);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            AssertEventIsRouted<CheckoutSessionCompletedEvent>();
        }

        [Fact]
        public async Task InvoicePaid()
        {
            var @event = CreateAMockEvent(Events.InvoicePaid);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            AssertEventIsRouted<InvoicePaymentSuccessfulEvent>();
        }

        [Fact]
        public async Task InvoicePaymentFailed()
        {
            var @event = CreateAMockEvent(Events.InvoicePaymentFailed);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            AssertEventIsRouted<InvoicePaymentFailedEvent>();
        }

        [Fact]
        public async Task PaymentMethodUpdated()
        {
            var @event = CreateAMockEvent(Events.PaymentMethodUpdated);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            AssertEventIsRouted<PaymentMethodUpdatedEvent>();
        }

        [Fact]
        public async Task PlanUpdated()
        {
            var @event = CreateAMockEvent(Events.PlanUpdated);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            AssertEventIsRouted<PlanUpdatedEvent>();
        }

        [Fact]
        public async Task ChargeRefunded()
        {
            var @event = CreateAMockEvent(Events.ChargeRefunded);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            var mediator = (IGetData)Container.GetService<IMediator>();
            mediator.GetFiltered().Count.ShouldBe(0);
        }

        [Fact]
        public async Task CustomerCreated()
        {
            var @event = CreateAMockEvent(Events.CustomerCreated);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            var mediator = (IGetData)Container.GetService<IMediator>();
            mediator.GetFiltered().Count.ShouldBe(0);
        }

        [Fact]
        public async Task InvoiceCreated()
        {
            var @event = CreateAMockEvent(Events.InvoiceCreated);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            AssertEventIsRouted<StripeInvoiceCreatedEvent>();
        }

        [Fact]
        public async Task PlanDeleted()
        {
            var @event = CreateAMockEvent(Events.PlanDeleted);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            var mediator = (IGetData)Container.GetService<IMediator>();
            mediator.GetFiltered().Count.ShouldBe(0);
        }

        [Fact]
        public async Task PriceUpdated()
        {
            var @event = CreateAMockEvent(Events.PriceUpdated);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            AssertEventIsRouted<PriceUpdatedEvent>();
        }

        public override Task InitializeAsync()
        {
            router = Container.GetService<IStripeEventWebhookRoutingService>();
            ManuallySetCancellationToken();
            return base.InitializeAsync();
        }

        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.Register(
                    c =>
                    {
                        var serviceFactory = new ServiceFactory(type => { return Container.GetService(type); });
                        var decorator = new MediatorDecorator(serviceFactory);
                        return decorator;
                    })
                .As<IMediator>()
                .SingleInstance();
            return base.CustomizeContainer(builder);
        }

        private void AssertEventIsRouted<TEventHandler>()
        {
            var mediator = (IGetData)Container.GetService<IMediator>();
            mediator.GetFiltered()[0].Name.ShouldBe(typeof(TEventHandler).Name);
        }

        private Event CreateAMockEvent(string eventType)
        {
            var @event = new Event
            {
                Id = A.RandomName(),
                Type = eventType,
                Data = new EventData()
                {
                    Object = null
                }
            };

            return @event;
        }

        private async Task<Event> WriteAMockEvent(string eventType, string? signature = null)
        {
            var @event = CreateAMockEvent(eventType);
            await AccountRepository.AddStripeEvent(@event.Id, signature);
            return @event;
        }


        public interface IGetData
        {
            List<Type> Get();
            List<Type> GetFiltered();
        }

        public class MediatorDecorator : IMediator, IGetData
        {
            private readonly IMediator mediator;

            public MediatorDecorator(ServiceFactory factory)
            {
                mediator = new Mediator(factory);
            }

            public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
            {
                await Task.CompletedTask;
                ReceivedTypes.Add(request.GetType());

                if (request.GetType().Name == nameof(NewStripeEventReceivedEvent))
                {
                    var result = await mediator.Send(request, cancellationToken);
                    return result;
                }

                return default;
            }

            public async Task<object?> Send(object request, CancellationToken cancellationToken = new CancellationToken())
            {
                await Task.CompletedTask;
                ReceivedTypes.Add(request.GetType());
                return default;
            }

            public async Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken())
            {
                await Task.CompletedTask;
                ReceivedTypes.Add(notification.GetType());
            }

            public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = new CancellationToken()) where TNotification : INotification
            {
                await Task.CompletedTask;

                ReceivedTypes.Add(notification.GetType());
            }

            public List<Type> Get()
            {
                return ReceivedTypes;
            }

            public List<Type> GetFiltered()
            {
                return FilterHeadAndTail(Get());
            }

            private readonly string first = nameof(NewStripeEventReceivedEvent);
            private readonly string last = nameof(StripeEventProcessedSuccessfullyEvent);

            public List<Type> FilterHeadAndTail(List<Type> types)
            {
                return types.Where(x => x.Name != first && x.Name != last).ToList();
            }

            public List<Type> ReceivedTypes { get; set; } = new List<Type>();
        }
    }
}