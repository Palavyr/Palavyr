using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Palavyr.Component.ComponentTestBase;
using Palavyr.Component.Mocks;
using Palavyr.Core.Data;
using Palavyr.Core.Handlers.StripeWebhookHandlers.PaymentFailed;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Test.Common.ApprovalTests;
using Test.Common.Builders.Accounts;
using Test.Common.Builders.StripeBuilders;
using Test.Common.Random;
using Xunit;

namespace Palavyr.Component.Tests.StripeWebhookHandlers
{
    public class ProcessStripeInvoicePaymentFailedHandlerFixture : ComponentTest
    {
        [Fact]
        public async Task ProcessStripeInvoicePaymentFailIsHandled()
        {
            var customerId = A.RandomId();
            var account = await new AccountObjectBuilder()
                .WithAccountId(AccountId)
                .WithStripeCustomerId(customerId)
                .BuildAndMakeRaw(ResolveType<AppDataContexts>(), CancellationToken);

            var invoice = new StripeInvoiceBuilder(this).WithCustomerId(account.StripeCustomerId).Build();
            var @event = new InvoicePaymentFailedEvent(invoice);
            var handler = ResolveType<INotificationHandler<InvoicePaymentFailedEvent>>();

            await handler.Handle(@event, CancellationToken);

            var result = (IGetEmailSent)ResolveType<ISesEmail>();
            this.PalavyrAssent(result.GetSentHtml(), ignoreLines: new List<int>() { 33 });
        }

        public override void OverrideCustomization(ContainerBuilder fixtureUnBuiltContainer)
        {
            base.OverrideCustomization(fixtureUnBuiltContainer);
            fixtureUnBuiltContainer.RegisterType<MockSeSEmail>().As<ISesEmail>().SingleInstance();
        }

        public ProcessStripeInvoicePaymentFailedHandlerFixture(ComponentClassFixture fixture) : base(fixture)
        {
            TransportsEnabled = false;
        }
    }
}