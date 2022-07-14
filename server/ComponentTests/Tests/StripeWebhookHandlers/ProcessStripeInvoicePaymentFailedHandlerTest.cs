using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class ProcessStripeInvoicePaymentFailedHandlerTest : ComponentTest
    {
        [Fact]
        public async Task HandlesEvent()
        {
            var customerId = A.RandomId();
            await new AccountObjectBuilder()
                .WithAccountId(AccountId)
                .WithStripeCustomerId(customerId)
                .BuildAndMakeRaw(ResolveType<AccountsContext>(), CancellationToken);

            var invoice = new StripeInvoiceBuilder(this).WithCustomerId(customerId).Build();
            var @event = new InvoicePaymentFailedEvent(invoice);

            var handler = ResolveType<INotificationHandler<InvoicePaymentFailedEvent>>();
            await handler.Handle(@event, CancellationToken);

            var result = ResolveType<ISesEmail>() as IGetEmailSent;

            this.PalavyrAssent(result?.GetSentHtml(), ignoreLines: new List<int>(){ 33 });
        }

        public ProcessStripeInvoicePaymentFailedHandlerTest(ComponentClassFixture fixture) : base(fixture)
        {
            TransportsEnabled = false;
        }
    }
}