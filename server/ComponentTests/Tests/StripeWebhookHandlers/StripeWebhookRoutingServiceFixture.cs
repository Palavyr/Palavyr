using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Component.ComponentTestBase;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Handlers.StripeWebhookHandlers.InvoiceCreated;
using Palavyr.Core.Handlers.StripeWebhookHandlers.InvoicePaid;
using Palavyr.Core.Handlers.StripeWebhookHandlers.PaymentFailed;
using Palavyr.Core.Handlers.StripeWebhookHandlers.SubscriptionCreated;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;
using Shouldly;
using Stripe;
using Test.Common.Random;
using Xunit;

namespace Component.Tests.StripeWebhookHandlers
{
    public class StripeWebhookRoutingServiceFixture : ComponentTest
    {
        private IStripeEventWebhookRoutingService router = null!;

        public StripeWebhookRoutingServiceFixture(ComponentClassFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task EventReceivedIsFired()
        {
            var signature = A.RandomId();
            var stripeWebhookStore = ResolveStore<StripeWebhookReceivedRecord>();
            var @event = await WriteAMockEvent(A.RandomName(), stripeWebhookStore, signature);

            await router.ProcessStripeEvent(@event, signature, CancellationToken.None);

            var mediator = (IGetData)ResolveType<IMediator>();
            mediator.Get().First().Name.ShouldBe(nameof(NewStripeEventReceivedEvent));
        }

        [Fact]
        public async Task SkipsEvent()
        {
            var signature = A.RandomId();
            var stripeWebhookStore = ResolveStore<StripeWebhookReceivedRecord>();
            var @event = await WriteAMockEvent(A.RandomName(), stripeWebhookStore, signature);

            await router.ProcessStripeEvent(@event, signature, CancellationToken.None);

            var mediator = (IGetData)ResolveType<IMediator>();
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

            AssertEventIsRouted<CheckoutSessionCompletedNotification>();
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

            var mediator = (IGetData)ResolveType<IMediator>();
            mediator.GetFiltered().Count.ShouldBe(0);
        }

        [Fact]
        public async Task CustomerCreated()
        {
            var @event = CreateAMockEvent(Events.CustomerCreated);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            var mediator = (IGetData)ResolveType<IMediator>();
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

            var mediator = (IGetData)ResolveType<IMediator>();
            mediator.GetFiltered().Count.ShouldBe(0);
        }

        [Fact]
        public async Task PriceUpdated()
        {
            var @event = CreateAMockEvent(Events.PriceUpdated);

            await router.ProcessStripeEvent(@event, A.RandomId(), CancellationToken);

            AssertEventIsRouted<PriceUpdatedEvent>();
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            router = ResolveType<IStripeEventWebhookRoutingService>();
            SetCancellationToken();
        }


        public override void OverrideCustomization(ContainerBuilder fixtureUnBuiltContainer)
        {
            base.OverrideCustomization(fixtureUnBuiltContainer);
            fixtureUnBuiltContainer.Register(
                    c =>
                    {
                        var serviceFactory = new ServiceFactory(type => { return Container.Resolve(type); });
                        var decorator = new MediatorDecorator(serviceFactory);
                        return decorator;
                    })
                .As<IMediator>()
                .SingleInstance();
        }


        private void AssertEventIsRouted<TEventHandler>()
        {
            var mediator = (IGetData)ResolveType<IMediator>();
            mediator.GetFiltered().First().Name.ShouldBe(typeof(TEventHandler).Name);
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

        private async Task<Event> WriteAMockEvent(string eventType, IEntityStore<StripeWebhookReceivedRecord> stripeWebhookStore, string? signature = null)
        {
            var @event = CreateAMockEvent(eventType);
            await stripeWebhookStore.AddStripeEvent(@event.Id, signature!);
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

                return default!;
            }

            public async Task<object?> Send(object request, CancellationToken cancellationToken = new CancellationToken())
            {
                await Task.CompletedTask;
                ReceivedTypes.Add(request.GetType());
                return default;
            }

            public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
            {
                throw new NotImplementedException();
            }

            public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = new CancellationToken())
            {
                throw new NotImplementedException();
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