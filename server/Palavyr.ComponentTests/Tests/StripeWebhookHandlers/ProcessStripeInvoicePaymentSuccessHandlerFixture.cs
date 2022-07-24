using System;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Component.ComponentTestBase;
using Palavyr.Component.Mocks;
using Palavyr.Core.Data;
using Palavyr.Core.Handlers.StripeWebhookHandlers.InvoicePaid;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Test.Common.ApprovalTests;
using Test.Common.Builders.Accounts;
using Test.Common.Builders.StripeBuilders;
using Test.Common.Random;
using Xunit;

namespace Palavyr.Component.Tests.StripeWebhookHandlers
{
    public class ProcessStripeInvoicePaymentSuccessHandlerTest : ComponentTest
    {
        [Fact]
        public async Task Handles()
        {
            var customerId = A.RandomId();
            await new AccountObjectBuilder()
                .WithAccountId(AccountId)
                .WithStripeCustomerId(customerId)
                .BuildAndMakeRaw(ResolveType<AppDataContexts>(), CancellationToken);

            var subscription = new StripeSubscriptionBuilder(this)
                .WithCustomerId(customerId)
                .WithCurrentPeriodEnd(DateTime.Now.AddDays(15))
                .Build();
            var invoice =  new StripeInvoiceBuilder(this)
                .WithCustomerId(customerId)
                .WithSubscription(subscription)
                .Build();

            var @event = new InvoicePaymentSuccessfulEvent(invoice);
            var handler = ResolveType<INotificationHandler<InvoicePaymentSuccessfulEvent>>();
            await handler.Handle(@event, CancellationToken);

            var result = ResolveType<ISesEmail>() as IGetEmailSent;
            this.PalavyrAssent(result?.GetSentHtml());
        }


        public ProcessStripeInvoicePaymentSuccessHandlerTest(ComponentClassFixture fixture) : base(fixture)
        {
            TransportsEnabled = false;
        }
    }
}