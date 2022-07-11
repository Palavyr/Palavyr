using System;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Handlers.StripeWebhookHandlers.InvoiceCreated;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.DataCreators.StripeBuilders;
using Palavyr.IntegrationTests.Mocks;
using Test.Common.ApprovalTests;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripeInvoiceCreatedHandlerTest : StripeServiceTestBaseBase
    {
        [Fact]
        public async Task Handles()
        {
            var subscription = this.CreateStripeSubscriptionBuilder()
                .WithCurrentPeriodEnd(DateTime.Now.AddDays(15))
                .Build();
            var invoice = await this.CreateStripeInvoiceBuilder()
                .WithCurrency("$")
                .WithAmountDue(150)
                .WithDueDate(DateTime.Parse("01/01/3000"))
                .WithSubscription(subscription)
                .Build();

            var @event = new StripeInvoiceCreatedEvent(invoice);
            var emailer = Container.GetService<ISesEmail>();
            var logger = Container.GetService<ILogger<ProcessStripeInvoiceCreatedHandler>>();
            var handler = ResolveType<INotificationHandler<StripeInvoiceCreatedEvent>>();

            await handler.Handle(@event, CancellationToken);

            var ses = (IGetEmailSent)Container.GetService<ISesEmail>();

            this.PalavyrAssent(ses.GetSentHtml());
        }

        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<MockSeSEmail>().As<ISesEmail>().SingleInstance();
            return base.CustomizeContainer(builder);
        }

        public ProcessStripeInvoiceCreatedHandlerTest(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }
    }
}