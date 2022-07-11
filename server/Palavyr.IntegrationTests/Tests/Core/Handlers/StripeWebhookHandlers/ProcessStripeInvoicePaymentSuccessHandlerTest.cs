using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Handlers.StripeWebhookHandlers.InvoicePaid;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.DataCreators.StripeBuilders;
using Palavyr.IntegrationTests.Mocks;
using Test.Common.ApprovalTests;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripeInvoicePaymentSuccessHandlerTest : StripeServiceTestBaseBase
    {
        [Fact]
        public async Task Handles()
        {
            var subscription = this.CreateStripeSubscriptionBuilder()
                .WithCurrentPeriodEnd(DateTime.Now.AddDays(15))
                .Build();
            var invoice = await this.CreateStripeInvoiceBuilder()
                .WithSubscription(subscription)
                .Build();

            var @event = new InvoicePaymentSuccessfulEvent(invoice);
            var emailer = Container.GetService<ISesEmail>();
            var logger = Container.GetService<ILogger<ProcessStripeInvoicePaymentSuccessHandler>>();
            var handler = new ProcessStripeInvoicePaymentSuccessHandler(ResolveStore<Account>(), logger, emailer);

            await handler.Handle(@event, CancellationToken);

            var ses = (IGetEmailSent)Container.GetService<ISesEmail>();

            this.PalavyrAssent(ses.GetSentHtml());
        }

        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<MockSeSEmail>().As<ISesEmail>().SingleInstance();
            return base.CustomizeContainer(builder);
        }

        public ProcessStripeInvoicePaymentSuccessHandlerTest(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }
    }
}