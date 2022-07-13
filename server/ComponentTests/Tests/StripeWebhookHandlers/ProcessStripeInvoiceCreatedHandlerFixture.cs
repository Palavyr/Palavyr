using System;
using System.Threading.Tasks;
using Component.AppFactory.ComponentTestBase.BaseFixture;
using Component.Mocks;
using MediatR;
using Palavyr.Core.Data;
using Palavyr.Core.Handlers.StripeWebhookHandlers.InvoiceCreated;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Test.Common.ApprovalTests;
using Test.Common.Builders.Accounts;
using Test.Common.Builders.StripeBuilders;
using Test.Common.Random;
using Xunit;

namespace Component.Tests.StripeWebhookHandlers
{
    public class ProcessStripeInvoiceCreatedHandlerTest : ComponentTest
    {
        [Fact]
        public async Task Handles()
        {
            var customerId = A.RandomId();
            await new AccountObjectBuilder()
                .WithAccountId(AccountId)
                .WithStripeCustomerId(customerId)
                .BuildAndMakeRaw(ResolveType<AccountsContext>(), CancellationToken);

            var subscription = new StripeSubscriptionBuilder(this)
                .WithCurrentPeriodEnd(DateTime.Now.AddDays(15))
                .Build();
            var invoice = new StripeInvoiceBuilder(this)
                .WithCurrency("$")
                .WithAmountDue(150)
                .WithCustomerId(customerId)
                .WithDueDate(DateTime.Parse("01/01/3000"))
                .WithSubscription(subscription)
                .Build();

            var @event = new StripeInvoiceCreatedEvent(invoice);
            var handler = ResolveType<INotificationHandler<StripeInvoiceCreatedEvent>>();

            await handler.Handle(@event, CancellationToken);

            var result = ResolveType<ISesEmail>() as IGetEmailSent;
            this.PalavyrAssent(result?.GetSentHtml());
        }

        public ProcessStripeInvoiceCreatedHandlerTest(ComponentClassFixture fixture) : base(fixture)
        {
            TransportsEnabled = false;
        }
    }
}