using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.PaymentFailed;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.DataCreators.StripeBuilders;
using Palavyr.IntegrationTests.Tests.Mocks;
using Test.Common.ApprovalTests;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripeInvoicePaymentFailedHandlerFixture : StripeServiceFixtureBase
    {
        [Fact]
        public async Task HandlesEvent()
        {
            var invoice = await this.CreateStripeInvoiceBuilder().Build();
            var @event = new InvoicePaymentFailedEvent(invoice);
            var emailer = Container.GetService<ISesEmail>();
            var logger = Container.GetService<ILogger<ProcessStripeInvoicePaymentFailedHandler>>();
            var handler = new ProcessStripeInvoicePaymentFailedHandler(logger, ResolveStore<Account>(), emailer);

            await handler.Handle(@event, CancellationToken);

            var ses = (IGetEmailSent)Container.GetService<ISesEmail>();
 
            this.PalavyrAssent(ses.GetSentHtml());
        }

        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<MockSeSEmail>().As<ISesEmail>().SingleInstance();
            return base.CustomizeContainer(builder);
        }

        public ProcessStripeInvoicePaymentFailedHandlerFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }
    }
}