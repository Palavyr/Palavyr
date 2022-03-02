﻿using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.InvoiceCreated;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.DataCreators;
using Palavyr.IntegrationTests.Tests.Mocks;
using Test.Common.ApprovalTests;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripeInvoiceCreatedHandlerFixture : StripeServiceFixtureBase
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
                .WithDueDate(DateTime.Now.AddDays(7))
                .WithSubscription(subscription)
                .Build();

            var @event = new StripeInvoiceCreatedEvent(invoice);
            var emailer = Container.GetService<ISesEmail>();
            var logger = Container.GetService<ILogger<ProcessStripeInvoiceCreatedHandler>>();
            var handler = new ProcessStripeInvoiceCreatedHandler(logger, AccountsContext, emailer);

            await handler.Handle(@event, CancellationToken);

            var ses = (IGetEmailSent)Container.GetService<ISesEmail>();

            this.PalavyrAssent(ses.GetSentHtml());
        }

        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<MockSeSEmail>().As<ISesEmail>().SingleInstance();
            return base.CustomizeContainer(builder);
        }

        public ProcessStripeInvoiceCreatedHandlerFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }
    }
}