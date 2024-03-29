﻿using System.Threading.Tasks;
using MediatR;
using Palavyr.Component.ComponentTestBase;
using Palavyr.Component.Mocks;
using Palavyr.Core.Data;
using Palavyr.Core.Handlers.StripeWebhookHandlers.PaymentFailed;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Shouldly;
using Test.Common.Builders.Accounts;
using Test.Common.Builders.StripeBuilders;
using Test.Common.Random;
using Xunit;

namespace Palavyr.Component.Tests.StripeWebhookHandlers
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
                .BuildAndMakeRaw(ResolveType<AppDataContexts>(), CancellationToken);

            var invoice = new StripeInvoiceBuilder(this).WithCustomerId(customerId).Build();
            var @event = new InvoicePaymentFailedEvent(invoice);

            var handler = ResolveType<INotificationHandler<InvoicePaymentFailedEvent>>();
            await handler.Handle(@event, CancellationToken);

            var result = (IGetEmailSent)ResolveType<ISesEmail>();
            
            result?.GetSentHtml().ShouldContain("Oh No! Your latest subscription payment has failed!");
            
            // this.PalavyrAssent(result?.GetSentHtml(), ignoreLines: new List<int>(){ 33 });
        }

        public ProcessStripeInvoicePaymentFailedHandlerTest(ComponentClassFixture fixture) : base(fixture)
        {
            TransportsEnabled = false;
        }
    }
}