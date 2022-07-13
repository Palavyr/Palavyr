using System.Threading.Tasks;
using Autofac;
using Component.AppFactory.ComponentTestBase.BaseFixture;
using Component.Mocks;
using MediatR;
using Palavyr.Core.Data;
using Palavyr.Core.Handlers.StripeWebhookHandlers.PaymentFailed;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Test.Common.ApprovalTests;
using Test.Common.Builders.Accounts;
using Test.Common.Builders.StripeBuilders;
using Test.Common.Random;
using Xunit;

namespace Component.Tests.StripeWebhookHandlers
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
                .BuildAndMakeRaw(ResolveType<AccountsContext>(), CancellationToken);

            var invoice = new StripeInvoiceBuilder(this).WithCustomerId(account.StripeCustomerId).Build();
            var @event = new InvoicePaymentFailedEvent(invoice);
            var handler = ResolveType<INotificationHandler<InvoicePaymentFailedEvent>>();

            await handler.Handle(@event, CancellationToken);

            var result = ResolveType<ISesEmail>() as IGetEmailSent;
            this.PalavyrAssent(result.GetSentHtml());
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